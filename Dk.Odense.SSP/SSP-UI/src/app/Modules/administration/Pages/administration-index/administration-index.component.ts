import { Component, OnInit } from '@angular/core';
import { Faset } from '@models/faset.model';
import { AreaRule } from '@domain-models/area-rule.model';
import { SourceService } from '@services/source.service';
import { PoliceWorryCategoryService } from '@services/police-worry-category.service';
import { PoliceWorryRoleService } from '@services/police-worry-role.service';
import { SspAreaService } from '@services/ssp-area.service';
import { CategorizationService } from '@services/categorization.service';
import { AreaRuleService } from '@services/area-rule.service';
import { Categorization } from '@models/categorization.model';
import { Classification } from '@domain-models/classification.model';
import { ClassificationService } from '@services/classification.service';

@Component({
  selector: 'app-administration-index',
  templateUrl: './administration-index.component.html',
  styleUrls: ['./administration-index.component.scss'],
})
export class AdministrationIndexComponent implements OnInit {
  sourceList: Faset[];
  typeList: Faset[];
  categoryList: Faset[];
  areaList: Faset[];
  areaRuleList: AreaRule[];
  expireCategorizationList!: Categorization[];
  classificationList: Faset[];

  constructor(
    private sourceService: SourceService,
    private typeService: PoliceWorryCategoryService,
    private categoryService: PoliceWorryRoleService,
    private areaService: SspAreaService,
    private expireCategoryService: CategorizationService,
    private areaRuleService: AreaRuleService,
    private classificationService: ClassificationService
  ) {
    this.sourceList = [];
    this.typeList = [];
    this.categoryList = [];
    this.areaList = [];
    this.areaRuleList = [];
    this.classificationList = [];
  }

  ngOnInit() {
    this.getSourceList();
    this.getTypeList();
    this.getCategoryList();
    this.getAreaList();
    this.getExpireCategoryList();
    this.getAreaRulesList();
    this.getClassificationList();
  }

  getClassificationList() {
    this.classificationService
      .getFasetList()
      .subscribe((x) => (this.classificationList = x));
  }

  // List Retrieval
  getSourceList() {
    this.sourceService.getFacetList().subscribe((r) => (this.sourceList = r));
  }

  getTypeList() {
    this.typeService.getFacetList().subscribe((r) => (this.typeList = r));
  }

  getCategoryList() {
    this.categoryService
      .getFacetList()
      .subscribe((r) => (this.categoryList = r));
  }

  getAreaList() {
    this.areaService.getFacetList().subscribe((r) => (this.areaList = r));
  }

  getExpireCategoryList() {
    this.expireCategoryService
      .getFacetList()
      .subscribe((r) => (this.expireCategorizationList = r));
  }

  getAreaRulesList() {
    this.areaRuleService.list().subscribe((r) => (this.areaRuleList = r));
  }

  // Sources
  sourceCreateEvent(value: string) {
    const source: Faset = new Faset();
    source.value = value;
    this.sourceService.post(source).subscribe((r) => this.sourceList.push(r));
  }

  sourceUpdateEvent(faset: Faset) {
    this.sourceService.put(faset).subscribe();
  }

  sourceDeleteEvent(faset: Faset) {
    faset.validUntil = new Date();
    this.sourceService.put(faset).subscribe((r) => this.getSourceList());
  }

  //Classifications
  classificationCreateEvent(value: string) {
    let classification = new Faset();
    classification.value = value;
    this.classificationService
      .post(classification)
      .subscribe((x) => this.classificationList.push(classification));
  }

  classificationUpdateEvent(classification: Classification) {
    this.classificationService.put(classification).subscribe();
  }

  classificationDeleteEvent(classification: Classification) {
    classification.validUntil = new Date();
    this.classificationService
      .put(classification)
      .subscribe((r) => this.getClassificationList());
  }

  // Types
  typeCreateEvent(value: string) {
    const type = new Faset();
    type.value = value;
    this.typeService.post(type).subscribe((r) => this.typeList.push(r));
  }

  typeUpdateEvent(faset: Faset) {
    this.typeService.put(faset).subscribe();
  }

  typeDeleteEvent(faset: Faset) {
    faset.validUntil = new Date();
    this.typeService.put(faset).subscribe((r) => this.getTypeList());
  }

  // Categories
  categoryCreateEvent(value: string) {
    const category = new Faset();
    category.value = value;
    this.categoryService
      .post(category)
      .subscribe((t) => this.categoryList.push(t));
  }

  categoryUpdateEvent(faset: Faset) {
    this.categoryService.put(faset).subscribe();
  }

  categoryDeleteEvent(faset: Faset) {
    faset.validUntil = new Date();
    this.categoryService.put(faset).subscribe((r) => this.getCategoryList());
  }

  // Areas
  areaCreateEvent(value: string) {
    const area = new Faset();
    area.value = value;
    this.areaService.post(area).subscribe((r) => this.areaList.push(r));
  }

  areaUpdateEvent(faset: Faset) {
    this.areaService.put(faset).subscribe();
  }

  areaDeleteEvent(faset: Faset) {
    faset.validUntil = new Date();
    this.areaService.put(faset).subscribe((r) => this.getAreaList());
  }

  // AreaRules
  areaRuleCreateEvent(value: AreaRule) {
    this.areaRuleService
      .post(value)
      .subscribe((r) => this.areaRuleList.push(r));
  }

  areaRuleUpdateEvent(value: AreaRule) {
    this.areaRuleService.put(value).subscribe();
  }

  areaRuleDeleteEvent(value: AreaRule) {
    this.areaRuleService.del(value).subscribe((r) => this.getAreaRulesList());
  }

  // ExpireCategory
  expireCategoryCreateEvent(faset: Faset) {
    this.expireCategoryService
      .post(faset)
      .subscribe((r) => this.expireCategorizationList.push(r));
  }

  expireCategoryUpdateEvent(faset: Faset) {
    this.expireCategoryService.put(faset).subscribe();
  }

  expireCategoryDeleteEvent(faset: Faset) {
    faset.validUntil = new Date();
    this.expireCategoryService
      .put(faset)
      .subscribe((r) => this.getExpireCategoryList());
  }
}
