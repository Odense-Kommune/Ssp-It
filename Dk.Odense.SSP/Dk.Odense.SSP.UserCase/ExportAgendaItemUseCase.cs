using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dk.Odense.SSP.Application.Interfaces;
using Dk.Odense.SSP.Core;
using Dk.Odense.SSP.Domain.Model;
using Dk.Odense.SSP.UserCase.ViewModel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace Dk.Odense.SSP.UserCase
{
    public class ExportAgendaItemUseCase
    {
        private readonly IAgendaItemService agendaItemService;

        private const int PageWidth = 100;
        private readonly TableWidthUnitValues TableWidthUnit = TableWidthUnitValues.Pct;
        private readonly CalcWeekNumber calcWeekNumber;

        public ExportAgendaItemUseCase(IAgendaItemService agendaItemService, CalcWeekNumber calcWeekNumber)
        {
            this.agendaItemService = agendaItemService;
            this.calcWeekNumber = calcWeekNumber;
        }

        public async Task<ExportData> CreateWordDocument(Guid agendaItemId, string rootPath)
        {
            var agendaItem = await agendaItemService.GetAgendaItemWithIncludes(agendaItemId);

            var path = rootPath + @"\Agendas";
            var fileName = @"DagsordenPunkt " + GetFileName(agendaItem.Agenda.Date, agendaItem.Worries.FirstOrDefault()?.Person.Name ?? "Ukendt") + ".docx";
            var file = new FileInfo(Path.Combine(path, fileName));

            MakeDir(path);
            DeleteFiles(path);

            CreateWordprocessingDocument(agendaItem, file);

            return new ExportData()
            {
                RootFolder = path,
                FileName = fileName,
                FileInfo = file
            };
        }

        private void CreateWordprocessingDocument(AgendaItem agendaItem, FileInfo filepath)
        {
            // Create a document by supplying the filepath. 
            using WordprocessingDocument wordDocument =
                WordprocessingDocument.Create(filepath.ToString(), WordprocessingDocumentType.Document);
            // Add a main document part. 
            MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png);
            GetImage(imagePart);

            // Create the document structure and add some text.
            mainPart.Document = new Document();
            Body body = mainPart.Document.AppendChild(new Body());

            var robustness = agendaItem.Worries.FirstOrDefault()?.Person.Robustnesses
                .OrderByDescending(x => x.CreatedDate).FirstOrDefault();

            body.AppendChild(AddImage(mainPart.GetIdOfPart(imagePart)));

            var table = CreateTable();

            table.AppendChild(LogoRow());

            table.AppendChild(PersonRow(agendaItem.Worries.FirstOrDefault()?.Person, agendaItem.Worries.FirstOrDefault()?.Person.Worries.Count ?? 0));
            table.AppendChild(TovholderRow(agendaItem.Worries.FirstOrDefault()?.Person?.SocialWorker));
            table.AppendChild(WorriesRow(agendaItem.Worries));
            table.AppendChild(NotesRow(agendaItem.Worries.FirstOrDefault()?.Person.Notes.Where(x=>x.Discriminator == "NoteShared")));
            table.AppendChild(AvaRecommendationRow());
            table.AppendChild(RobustnessRow(robustness, agendaItem.Worries));

            body.AppendChild(table);
            body.AppendChild(SendTo(robustness, agendaItem.Worries));
        }

        private TableRow TovholderRow(string personSocialWorker)
        {
            var tr = new TableRow();
            var tc = new TableCell();
            var para = new Paragraph();
            var runHeader = new Run();
            var runText = new Run();
            
            tc.AppendChild(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnit, Width = PageWidth.ToString() }, new GridSpan() { Val = 3 }));
            
            runHeader.AppendChild(StyleGray());
            runHeader.AppendChild(StyleBold());
            runHeader.AppendChild(StyleFont());
            runHeader.AppendChild(new Text("Tovholdernavn: "));
            runHeader.AppendChild(new Break());
            runText.AppendChild(StyleFont());
            runText.AppendChild(new Text(personSocialWorker));

            //Append
            para.AppendChild(runHeader);
            para.AppendChild(runText);
            tc.AppendChild(para);
            tr.AppendChild(tc);

            return tr;
        }

        private ImagePart GetImage(ImagePart imagePart)
        {
            using var stream = new FileStream(@".\Assets\logo.png", FileMode.Open);
            imagePart.FeedData(stream);

            return imagePart;
        }

        private static Drawing AddImage(string imageLogoHeader)
        {
            var imageWidth = 1440000L;
            var imageHeight = 420000L;
            var posX = 5300000L;
            var posY = 1130000L;

            Drawing drawing = new Drawing();
            DW.Anchor anchor2 = new DW.Anchor() { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)114300U, DistanceFromRight = (UInt32Value)114300U, SimplePos = true, RelativeHeight = (UInt32Value)251662848U, BehindDoc = false, Locked = false, LayoutInCell = false, AllowOverlap = true };
            DW.SimplePosition simplePosition2 = new DW.SimplePosition() { X = posX, Y = posY };
            DW.HorizontalPosition horizontalPosition2 = new DW.HorizontalPosition() { RelativeFrom = DW.HorizontalRelativePositionValues.Column };
            DW.PositionOffset positionOffset3 = new DW.PositionOffset();
            positionOffset3.Text = "0";
            horizontalPosition2.Append(positionOffset3);
            DW.VerticalPosition verticalPosition2 = new DW.VerticalPosition() { RelativeFrom = DW.VerticalRelativePositionValues.Paragraph };
            DW.PositionOffset positionOffset4 = new DW.PositionOffset();
            positionOffset4.Text = "32385";
            verticalPosition2.Append(positionOffset4);
            DW.Extent extent = new DW.Extent() { Cx = imageWidth, Cy = imageHeight };
            DW.EffectExtent effetctEx = new DW.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L, };
            DW.WrapTight wrapTight1 = new DW.WrapTight() { WrapText = DW.WrapTextValues.BothSides };
            DW.WrapPolygon wrapPolygon1 = new DW.WrapPolygon() { Edited = false };
            DW.StartPoint startPoint1 = new DW.StartPoint() { X = posX, Y = posY };
            DW.LineTo lineTo1 = new DW.LineTo() { X = 0L, Y = 21036L };
            DW.LineTo lineTo2 = new DW.LineTo() { X = 21551L, Y = 21036L };
            DW.LineTo lineTo3 = new DW.LineTo() { X = 21551L, Y = 0L };
            DW.LineTo lineTo4 = new DW.LineTo() { X = 0L, Y = 0L };
            wrapPolygon1.Append(startPoint1);
            wrapPolygon1.Append(lineTo1);
            wrapPolygon1.Append(lineTo2);
            wrapPolygon1.Append(lineTo3);
            wrapPolygon1.Append(lineTo4);
            wrapTight1.Append(wrapPolygon1);
            DW.DocProperties docProperties = new DW.DocProperties() { Id = (UInt32Value)1U, Name = "Header Logo" };
            DW.NonVisualGraphicFrameDrawingProperties nonVisualGraphicFrameDrawingProperties = new DW.NonVisualGraphicFrameDrawingProperties();
            A.GraphicFrameLocks graphicFrameLocks = new A.GraphicFrameLocks() { NoChangeAspect = true };
            A.Graphic graphic = new A.Graphic();
            A.GraphicData graphicData = new A.GraphicData() { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" };
            PIC.Picture picture = new PIC.Picture();
            PIC.NonVisualPictureProperties nonVisualPictureProperties = new PIC.NonVisualPictureProperties();
            PIC.NonVisualDrawingProperties nonVisualDrawingProperties = new PIC.NonVisualDrawingProperties() { Id = (UInt32Value)0U, Name = "HeadeLogo.jpeg" };
            PIC.NonVisualPictureDrawingProperties nonVisualPictureDrawingProperties = new PIC.NonVisualPictureDrawingProperties();
            PIC.BlipFill blipFill = new PIC.BlipFill();
            A.Blip blip = new A.Blip() { Embed = imageLogoHeader, CompressionState = A.BlipCompressionValues.Print };
            A.Stretch stretch = new A.Stretch();
            A.FillRectangle fillRectangle = new A.FillRectangle();
            PIC.ShapeProperties shapeProperties = new PIC.ShapeProperties(
           new A.Transform2D(new A.Offset() { X = 10000L, Y = 10000L }, new A.Extents() { Cx = imageWidth, Cy = imageHeight }),
           new A.PresetGeometry(new A.AdjustValueList()) { Preset = A.ShapeTypeValues.Rectangle });
            nonVisualPictureProperties.Append(nonVisualDrawingProperties);
            nonVisualPictureProperties.Append(nonVisualPictureDrawingProperties);
            picture.Append(nonVisualPictureProperties);
            blipFill.Append(blip);
            stretch.Append(fillRectangle);
            blipFill.Append(stretch);
            picture.Append(blipFill);
            picture.Append(shapeProperties);
            graphicData.Append(picture);
            graphic.Append(graphicData);
            anchor2.Append(simplePosition2);
            anchor2.Append(horizontalPosition2);
            anchor2.Append(verticalPosition2);
            anchor2.Append(extent);
            anchor2.Append(effetctEx);
            anchor2.Append(wrapTight1);
            anchor2.Append(docProperties);
            anchor2.Append(nonVisualGraphicFrameDrawingProperties);
            anchor2.Append(graphic);
            drawing.Append(anchor2);

            return drawing;
        }

        private static Table CreateTable()
        {
            var table = new Table();
            UInt32Value tableSize = 10;
            var tableDesign = new EnumValue<BorderValues>(BorderValues.BasicThinLines);

            var tblProp = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = tableDesign, Size = tableSize },
                    new BottomBorder() { Val = tableDesign, Size = tableSize },
                    new LeftBorder() { Val = tableDesign, Size = tableSize },
                    new RightBorder() { Val = tableDesign, Size = tableSize },
                    new InsideHorizontalBorder() { Val = tableDesign, Size = tableSize },
                    new InsideVerticalBorder() { Val = tableDesign, Size = tableSize }),
                new TableLook()
                {
                    NoHorizontalBand = OnOffValue.FromBoolean(false),
                    NoVerticalBand = OnOffValue.FromBoolean(true),
                    FirstRow = OnOffValue.FromBoolean(true),
                    FirstColumn = OnOffValue.FromBoolean(true),
                    LastRow = OnOffValue.FromBoolean(false),
                    LastColumn = OnOffValue.FromBoolean(false)
                }, new TableStyle() { Val = StringValue.FromString("Tabel-Gitter") });

            var tg = new TableGrid(new GridColumn(), new GridColumn());

            table.AppendChild<TableProperties>(tblProp);
            table.AppendChild(tg);
            return table;
        }

        private TableRow LogoRow()
        {
            var tr = new TableRow();
            var tc = new TableCell();
            var para = new Paragraph();
            var runHeader = new Run();
            var runText = new Run();

            tc.AppendChild(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnit, Width = PageWidth.ToString() }, new GridSpan() { Val = 3 }));

            runHeader.AppendChild(StyleGray());
            runHeader.AppendChild(StyleBold());
            runHeader.AppendChild(StyleFont());
            runHeader.AppendChild(new Text("AVA-koordinationen"));
            runHeader.AppendChild(new Break());

            runText.AppendChild(StyleFont());
            runText.AppendChild(new Text(DateTime.Now.Date.ToString("dd-MM-yyyy")));

            para.AppendChild(runHeader);
            para.AppendChild(runText);
            tc.AppendChild(para);
            tr.AppendChild(tc);

            return tr;
        }

        private TableRow PersonRow(Person person, int worryCount)
        {
            var tr = new TableRow();
            var tcLeft = new TableCell();
            var tcMid = new TableCell();
            var tcRight = new TableCell();
            var paraLeft = new Paragraph();
            var paraMid = new Paragraph();
            var paraRight = new Paragraph();
            var runHeaderLeft = new Run();
            var runHeaderMid = new Run();
            var runHeaderRight = new Run();
            var runFooterRight = new Run();
            var runTextLeft = new Run();
            var runTextMid = new Run();
            var runTextRight = new Run();

            //Style
            tcLeft.AppendChild(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnit, Width = (PageWidth / 3).ToString() }));
            tcMid.AppendChild(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnit, Width = (PageWidth / 4).ToString() }));
            tcRight.AppendChild(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnit, Width = (PageWidth / 2).ToString() }));

            //Left Column
            runHeaderLeft.AppendChild(StyleGray());
            runHeaderLeft.AppendChild(StyleBold());
            runHeaderLeft.AppendChild(StyleFont());
            runHeaderLeft.AppendChild(new Text("Navn: "));
            runHeaderLeft.AppendChild(new Break());
            runTextLeft.AppendChild(StyleFont());
            runTextLeft.AppendChild(new Text(person.Name));

            //Middle Column
            runHeaderMid.AppendChild(StyleGray());
            runHeaderMid.AppendChild(StyleBold());
            runHeaderMid.AppendChild(StyleFont());
            runHeaderMid.AppendChild(new Text("Cpr: "));
            runHeaderMid.AppendChild(new Break());
            runTextMid.AppendChild(StyleFont());
            runTextMid.AppendChild(new Text(person.SocialSecNum));

            //Right Column
            runHeaderRight.AppendChild(StyleGray());
            runHeaderRight.AppendChild(StyleBold());
            runHeaderRight.AppendChild(StyleFont());
            runHeaderRight.AppendChild(new Text() { Text = "Antal forhold på AVA-listen: ", Space = SpaceProcessingModeValues.Preserve });

            runTextRight.AppendChild(StyleFont());
            runTextRight.AppendChild(new Text(worryCount.ToString()));
            runTextRight.AppendChild(new Break());

            runFooterRight.AppendChild(StyleGray());
            runFooterRight.AppendChild(StyleSmal());
            runFooterRight.AppendChild(StyleFont());
            runFooterRight.AppendChild(new Text("(antal nulstilles efter 12 måneder uden forhold)"));

            //Append
            paraLeft.AppendChild(runHeaderLeft);
            paraLeft.AppendChild(runTextLeft);
            paraMid.AppendChild(runHeaderMid);
            paraMid.AppendChild(runTextMid);
            paraRight.AppendChild(runHeaderRight);
            paraRight.AppendChild(runTextRight);
            paraRight.AppendChild(runFooterRight);

            tcLeft.AppendChild(paraLeft);
            tcMid.AppendChild(paraMid);
            tcRight.AppendChild(paraRight);

            tr.AppendChild(tcLeft);
            tr.AppendChild(tcMid);
            tr.AppendChild(tcRight);

            return tr;
        }

        private TableRow WorriesRow(IEnumerable<Worry> worries)
        {
            var tr = new TableRow();
            var tc = new TableCell();
            var para = new Paragraph();
            var runHeader = new Run();
            var runText = new Run();
            var firstItem = true;

            tc.AppendChild(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnit, Width = PageWidth.ToString() }, new GridSpan() { Val = 3 }));

            runHeader.AppendChild(StyleGray());
            runHeader.AppendChild(StyleBold());
            runHeader.AppendChild(StyleFont());
            runHeader.AppendChild(new Text("Bekymringen/-erne:"));

            foreach (var worry in worries)
            {
                if (!firstItem)
                {
                    runText.AppendChild(new Break());
                }

                runText.AppendChild(new Break());
                runText.AppendChild(StyleFont());
                runText.AppendChild(new Text("Delt af ") { Space = SpaceProcessingModeValues.Preserve });

                if (worry.Reporter.Name != null) runText.AppendChild(new Text($"{worry.Reporter.Name.Trim() } - ") { Space = SpaceProcessingModeValues.Preserve });
                runText.AppendChild(new Text($"{worry.Reporter.Workplace.Trim()}: ") { Space = SpaceProcessingModeValues.Preserve });


                if (worry.Concern?.CrimeConcern != null)
                {
                    var strArray = worry.Concern.CrimeConcern.Split(@"\r\n");

                    foreach (var s in strArray)
                    {
                        runText.AppendChild(new Text($"{s}"));
                        runText.AppendChild(new Break());
                    }
                }
                else
                {
                    runText.AppendChild(new Text("Ingen beskrivelse"));
                    runText.AppendChild(new Break());
                }

                

                firstItem = false;
            }

            //Append
            para.AppendChild(runHeader);
            para.AppendChild(runText);
            tc.AppendChild(para);
            tr.AppendChild(tc);

            return tr;
        }

        private TableRow NotesRow(IEnumerable<Note> notes)
        {
            var usableNotes = notes.Where(x => x.CreatedAt > DateTime.Now.AddMonths(-3));

            var tr = new TableRow();
            var tc = new TableCell();
            var para = new Paragraph();
            var runHeader = new Run();
            var runText = new Run();
            var firstItem = true;

            tc.AppendChild(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnit, Width = PageWidth.ToString() }, new GridSpan() { Val = 3 }));

            runHeader.AppendChild(StyleGray());
            runHeader.AppendChild(StyleBold());
            runHeader.AppendChild(StyleFont());
            runHeader.AppendChild(new Text("Vidensdeling fra mødet:"));

            foreach (var note in usableNotes)
            {
                if (!firstItem)
                {
                    runText.AppendChild(new Break());
                }

                runText.AppendChild(StyleFont());
                runText.AppendChild(new Break());
                runText.AppendChild(new Text($"{note.CreatedAt.Date:dd-MM-yyyy}:"));
                runText.AppendChild(new Break());
                runText.AppendChild(new Text($"{note.Value}") { Space = SpaceProcessingModeValues.Preserve });

                firstItem = false;
            }

            //Append
            para.AppendChild(runHeader);
            para.AppendChild(runText);
            tc.AppendChild(para);
            tr.AppendChild(tc);

            return tr;
        }

        private TableRow AvaRecommendationRow()
        {
            var tr = new TableRow();
            var tc = new TableCell();
            var para = new Paragraph();
            var runHeader = new Run();
            var runText = new Run();

            tc.AppendChild(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnit, Width = PageWidth.ToString() }, new GridSpan() { Val = 3 }));

            runHeader.AppendChild(StyleGray());
            runHeader.AppendChild(StyleBold());
            runHeader.AppendChild(StyleFont());
            runHeader.AppendChild(new Text("Anbefaling fra AVA-koordinationen:"));

            runText.AppendChild(StyleFont());
            runText.AppendChild(new Break());
            runText.AppendChild(new Break());
            runText.AppendChild(new Break());

            //Append
            para.AppendChild(runHeader);
            para.AppendChild(runText);
            tc.AppendChild(para);
            tr.AppendChild(tc);

            return tr;
        }

        private TableRow RobustnessRow(Robustness robu, IEnumerable<Worry> worries)
        {
            var tr = new TableRow();
            var tc = new TableCell();
            var para = new Paragraph();
            var runHeader = new Run();
            var runText = new Run();

            tc.AppendChild(new TableCellProperties(
                new TableCellWidth { Type = TableWidthUnit, Width = PageWidth.ToString() }, new GridSpan() { Val = 3 }));

            runHeader.AppendChild(StyleGray());
            runHeader.AppendChild(StyleBold());
            runHeader.AppendChild(StyleFont());
            runHeader.AppendChild(new Text("Robusthedsskema er udfyldt:"));
            runHeader.AppendChild(new Break());

            runText.AppendChild(StyleFont());
            if (robu == null)
            {
                runText.AppendChild(new Text("Intet Robusthedsskema udfyldt"));
                runText.AppendChild(new Break());
            }
            else
            {
                MakeAssesmentHeader(runText, $"{robu.CreatedDate:dd-MM-yyyy}", $"{robu.Reporter.Workplace ?? "Ikke angivet"}", $"{robu.Reporter.Name ?? "Ikke oplyst"}");
                MakeAssesment(runText, "Social adfærd", robu.Assessment.SocialBehavior, robu.Assessment.SocialBehaviorElaborate);
                MakeAssesment(runText, "Gode, jævnaldrende venner", robu.Assessment.GoodFriends, robu.Assessment.GoodFriendsElaborate);
                MakeAssesment(runText, "Færdigheder og trivsel", robu.Assessment.Skills, robu.Assessment.SkillsElaborate);
                MakeAssesment(runText, "Forhold til rusmidler", robu.Assessment.DrugRelationship, robu.Assessment.DrugRelationshipElaborate);
                MakeAssesment(runText, "Drømme og/eller tanker om fremtiden", robu.Assessment.FutureDreams, robu.Assessment.FutureDreamsElaborate);
                MakeAssesment(runText, "Positiv opbakning fra forældre/værge", robu.Assessment.PositiveSupport, robu.Assessment.PositiveSupportElaborate);
            }

            runText.AppendChild(new Break());

            foreach (var worry in worries)
            {
                runText.AppendChild(StyleFont());
                MakeAssesmentHeader(runText, $"{worry.CreatedDate:dd-MM-yyyy}", $"{worry.Reporter.Workplace ?? "Afsender ikke angivet"}", $"{worry.Reporter.Name ?? "Ikke oplyst"}");

                if (worry.Assessment == null)
                {
                    runText.AppendChild(new Text("Ingen vurdering"));
                    runText.AppendChild(new Break());
                    continue;
                }

                MakeAssesment(runText, "Social adfærd", worry.Assessment.SocialBehavior, worry.Assessment.SocialBehaviorElaborate);
                MakeAssesment(runText, "Gode, jævnaldrende venner", worry.Assessment.GoodFriends, worry.Assessment.GoodFriendsElaborate);
                MakeAssesment(runText, "Færdigheder og trivsel", worry.Assessment.Skills, worry.Assessment.SkillsElaborate);
                MakeAssesment(runText, "Forhold til rusmidler", worry.Assessment.DrugRelationship, worry.Assessment.DrugRelationshipElaborate);
                MakeAssesment(runText, "Drømme og/eller tanker om fremtiden", worry.Assessment.FutureDreams, worry.Assessment.FutureDreamsElaborate);
                MakeAssesment(runText, "Positiv opbakning fra forældre/værge", worry.Assessment.PositiveSupport, worry.Assessment.PositiveSupportElaborate);
            }

            //Append
            para.AppendChild(runHeader);
            para.AppendChild(runText);
            tc.AppendChild(para);
            tr.AppendChild(tc);

            return tr;
        }

        private static void MakeAssesment(Run runText, string header, Core.Enum.Status value, string elab)
        {
            var transValue = "";

            switch (value.ToString())
            {
                case "Yellow":
                    transValue = "Gul";
                    break;
                case "Red":
                    transValue = "Rød";
                    break;
                case "Green":
                    transValue = "Grøn";
                    break;
                default:
                    transValue = value.ToString();
                    break;
            }

            runText.AppendChild(new Text($"{header}: ") { Space = SpaceProcessingModeValues.Preserve });
            runText.AppendChild(new Text($"{transValue}") { Space = SpaceProcessingModeValues.Preserve });

            var first = true;
            if (elab != string.Empty)
            {
                var strArray = elab.Split(@"\r\n");

                foreach (var s in strArray)
                {
                    if (s == "") continue;
                    if (first) runText.AppendChild(new Text($" - ") { Space = SpaceProcessingModeValues.Preserve });
                    runText.AppendChild(new Text($"{s}") { Space = SpaceProcessingModeValues.Preserve });
                    runText.AppendChild(new Break());
                    first = false;
                }
            }
            runText.AppendChild(new Break());
        }

        private static void MakeAssesmentHeader(Run runText, string date, string sender, string senderName)
        {
            runText.AppendChild(new Text($"{date}, {sender}, {senderName}") { Space = SpaceProcessingModeValues.Preserve });
            runText.AppendChild(new Break());
        }

        private Paragraph SendTo(Robustness robu, IEnumerable<Worry> worries)
        {
            var para = new Paragraph();
            var runHeader = new Run();
            var runText = new Run();

            var runHeader2 = new Run();
            var runText2 = new Run();


            //runHeader.AppendChild(StyleGray());
            //runHeader.AppendChild(StyleBold());
            //runHeader.AppendChild(StyleFont());
            //runHeader.AppendChild(new Text("Tilbagemelding på Robusthedsskema sendes til: "));


            //runText.AppendChild(StyleFont());
            //runText.AppendChild(new Break());

            //if (robu != null)
            //{
            //    runText.AppendChild(new Text($"{robu.Reporter.ImmediateLeader}, {robu.Reporter.ImmediateLeaderEmail}, {robu.Reporter.ImmediateLeaderPhone}") { Space = SpaceProcessingModeValues.Preserve });
            //}
            
            runHeader2.AppendChild(StyleGray());
            runHeader2.AppendChild(StyleBold());
            runHeader2.AppendChild(StyleFont());
            runHeader2.AppendChild(new Break());
            runHeader2.AppendChild(new Text("Tilbagemelding på Kriminalitetsbekymring sendes til: "));

            runText2.AppendChild(StyleFont());

            runText2.AppendChild(new Break());

            if (robu?.CreatedDate > worries.OrderByDescending(x=>x.CreatedDate).FirstOrDefault()?.CreatedDate)
            {
                runText2.AppendChild(
                    new Text(
                            $"{robu.Reporter.ImmediateLeader ?? robu.Reporter.Workplace ?? "Navn ikke angivet"}, {robu.Reporter.ImmediateLeaderEmail ?? "Email ikke angivet"}, {robu.Reporter.ImmediateLeaderPhone ?? "Telefonnummer ikke angivet"}")
                        { Space = SpaceProcessingModeValues.Preserve });

            }
            else
            {
                var worry = worries.OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                runText2.AppendChild(
                    new Text(
                            $"{worry.Reporter.ImmediateLeader ?? worry.Reporter.Workplace ?? "Navn ikke angivet"}, {worry.Reporter.ImmediateLeaderEmail ?? "Email ikke angivet"}, {worry.Reporter.ImmediateLeaderPhone ?? "Telefonnummer ikke angivet"}")
                        { Space = SpaceProcessingModeValues.Preserve });
            }

            //Append
            para.AppendChild(runHeader);
            para.AppendChild(runText);

            para.AppendChild(runHeader2);
            para.AppendChild(runText2);

            return para;
        }

        #region Styles
        private RunProperties StyleGray()
        {
            return new RunProperties() { Color = new Color() { Val = "808080" } };
        }

        private RunProperties StyleBold()
        {
            return new RunProperties() { Bold = new Bold() { Val = true } };
        }

        private RunProperties StyleSmal()
        {
            return new RunProperties() { FontSize = new FontSize() { Val = "18" } };
        }

        private RunProperties StyleFont()
        {
            var runProp = new RunProperties();
            var runFont = new RunFonts() { HighAnsi = "Arial" };
            var runFont2 = new RunFonts() { Ascii = "Arial" };
            runProp.AppendChild(runFont);
            runProp.AppendChild(runFont2);

            return runProp;
        }
        #endregion

        #region IO Methods
        private void MakeDir(string path)
        {
            var directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists) Directory.CreateDirectory(path);
        }

        private static void DeleteFiles(string path)
        {
            var dir = new DirectoryInfo(path);
            foreach (var fileInfo in dir.GetFiles())
            {
                fileInfo.Delete();
            }
        }

        private string GetFileName(DateTime agendaDate, string personName)
        {
            var week = calcWeekNumber.GetWeekNumber(agendaDate);

            return "Uge " + week + " - " + personName;
        }
        #endregion
    }
}
