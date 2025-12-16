import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ReportedPerson } from '@domain-models/reported-person.model';
import { SocialSecData } from '@models/SocialSecData.model';
import { PersonService } from '@services/person.service';

@Component({
  selector: 'app-reporting-of',
  templateUrl: './reporting-of.component.html',
  styleUrls: ['./reporting-of.component.scss'],
})
export class ReportingOfComponent implements OnInit {
  @Input() person!: ReportedPerson;
  @Input() socialSecData!: SocialSecData;
  socialSecNum?: string;
  searchDisabled: boolean;
  @Output() pushSocialSecNum = new EventEmitter<{
    socialSecData: SocialSecData;
    socialSecNum: string;
  }>();

  constructor(private personService: PersonService) {
    this.searchDisabled = false;
  }

  getSocialSecData(socialSecNum: string): void {
    this.searchDisabled = true;
    this.personService.getSocialSecData(socialSecNum).subscribe((x) => {
      this.socialSecData = x;
      this.pushData(x);
      this.searchDisabled = false;
    });
  }

  pushData(socialSecData: SocialSecData) {
    let socialSecNum = socialSecData.socialSecNum;
    this.pushSocialSecNum.emit({ socialSecData, socialSecNum });
  }

  ngOnInit() {
    this.loadData();
  }

  ngOnChanges() {
    this.loadData();
  }

  loadData() {
    this.socialSecNum =
      this.socialSecData.socialSecNum !== null
        ? this.socialSecData.socialSecNum
        : this.person?.reportedCpr;
  }
}
