import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { VerifyWorry } from '@models/verify-worry.model';
import { VerifyWorryMenuItem } from '@models/verify-worry-menu-item.model';
import { SocialSecData } from '@models/SocialSecData.model';
import { VerifyWorryService } from '@services/verify-worry.service';
import { ToastService } from '@services/toast.service';

@Component({
  selector: 'app-verify-index',
  templateUrl: './verify-index.component.html',
  styleUrls: ['./verify-index.component.scss'],
})
export class VerifyIndexComponent implements OnInit {
  @ViewChild('scrollable1', { static: true }) scroll1?: ElementRef;
  @ViewChild('scrollable2', { static: true }) scroll2?: ElementRef;
  @ViewChild('scrollable3', { static: true }) scroll3?: ElementRef;
  selectedWorryName = '';
  selectedWorry?: VerifyWorry;
  selectedWorryId = '';
  selectedMenuItem = new VerifyWorryMenuItem();
  menuItems = new Array<VerifyWorryMenuItem>();
  newSocialSecNum?: string;
  newSocialSecData = new SocialSecData();
  disableApprove = true;
  disableGroundless = true;
  pendingItems = 0;

  constructor(
    private verifyWorryService: VerifyWorryService,
    private toastService: ToastService
  ) {
    this.menuItems = [];
  }

  errorToast() {
    this.toastService.createToast(
      'Der opstod en fejl under handlingen',
      'danger'
    );
  }

  selectWorry(menuItem: VerifyWorryMenuItem): void {
    this.selectedWorryName = '#' + menuItem.increment;
    this.selectedWorryId = menuItem.id;
    this.selectedMenuItem = menuItem;
    //If there is no socialSecNum, there is no Person object in the database yet
    //In that case, use reportedCpr, because that's the equivalent value on the ReportedPerson object
    this.newSocialSecNum =
      menuItem.socialSecData.socialSecNum ??
      menuItem.reportedPerson.reportedCpr ??
      undefined;
    this.newSocialSecData = menuItem.socialSecData;

    this.setDisableApprove();
    this.setDisableGroundless();

    this.getWorry(this.selectedWorryId);
  }

  setDisableApprove() {
    if (this.selectedMenuItem.approved === true) {
      this.disableApprove = false;
    } else {
      this.disableApprove = true;
    }
  }

  setDisableGroundless() {
    if (this.selectedMenuItem.groundless === true) {
      this.disableGroundless = false;
    } else {
      this.disableGroundless = true;
    }
  }

  getWorry(id: string): void {
    this.verifyWorryService.getVerifyWorry(id).subscribe((worry) => {
      this.selectedWorry = worry;
    });
  }

  getMenuItems(): void {
    this.verifyWorryService.getMenuItems().subscribe((menuItems) => {
      this.menuItems = menuItems;
    });
  }

  getApprovedMenuItems(): VerifyWorryMenuItem[] {
    return this.menuItems.filter((x) => x.approved === true);
  }

  getGroundlessMenuItems(): VerifyWorryMenuItem[] {
    return this.menuItems.filter((x) => x.groundless === true);
  }

  getNoActionMenuItems(): VerifyWorryMenuItem[] {
    return this.menuItems.filter(
      (x) => x.approved === false && x.groundless === false
    );
  }

  setGroundless() {
    if (confirm('Denne kriminalitetsbekymring vil blive slettet.')) {
      let result: Boolean;

      this.verifyWorryService
        .setGroundless(this.selectedMenuItem.id)
        .subscribe((x) => {
          result = x;
          if (result === false) {
            return;
          }
          this.toastService.createToast(
            'Bekymringer er blevet erklæret grundløs',
            'success'
          );
          this.selectedMenuItem.groundless = true;
          this.selectedMenuItem.approved = false;
        });
      return;
    }
  }

  setApproved() {
    let result: Boolean;

    if (this.newSocialSecNum == null) {
      this.toastService.createToast('Personen er ikke kendt i CPR', 'danger');
      return;
    }

    this.verifyWorryService
      .setApproved(this.selectedMenuItem.id, this.newSocialSecNum)
      .subscribe({
        next: (x) => {
          result = x;
          if (result === false) {
            this.errorToast();
            return;
          }

          this.selectedMenuItem.groundless = false;
          this.selectedMenuItem.approved = true;
          this.selectedMenuItem.socialSecData = this.newSocialSecData;

          this.toastService.createToast('Det kører bare for dig!', 'success');
        },
        error: () => {
          this.errorToast();
        },
      });

    return;
  }

  getNewSocialSecNum(data: {
    socialSecNum: string;
    socialSecData: SocialSecData;
  }) {
    this.newSocialSecNum = data.socialSecNum;
    this.newSocialSecData = data.socialSecData;
  }

  scrollableClick(event: any) {
    const scroll1: Element = this.scroll1?.nativeElement;
    const scroll2: Element = this.scroll2?.nativeElement;
    const scroll3: Element = this.scroll3?.nativeElement;
    const src = this.getCorrectSource(event.srcElement)?.nextSibling;
    if (src === scroll1) {
      this.recollapse(scroll1);
    }
    if (src === scroll2) {
      this.recollapse(scroll2);
    }
    if (src === scroll3) {
      this.recollapse(scroll3);
    }
  }

  recollapse(sourceElement: Element) {
    const scroll1: Element = this.scroll1?.nativeElement;
    const scroll2: Element = this.scroll2?.nativeElement;
    const scroll3: Element = this.scroll3?.nativeElement;
    scroll1.classList.add('collapsed');
    scroll2.classList.add('collapsed');
    scroll3.classList.add('collapsed');
    sourceElement.classList.remove('collapsed');
  }

  getCorrectSource(element: Element) {
    if (element.nodeName === 'H6') {
      return element.parentElement;
    }
    return element;
  }

  getPendingItems() {
    this.verifyWorryService
      .getPendingNumber()
      .subscribe((r) => (this.pendingItems = r));
  }

  ngOnInit() {
    this.getMenuItems();
    this.getPendingItems();
  }

  onUpdateDataEvent(data: string) {
    this.getMenuItems();
    this.getPendingItems();
  }
}
