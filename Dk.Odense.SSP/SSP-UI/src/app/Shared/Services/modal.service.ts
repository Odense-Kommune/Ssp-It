import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private modalList: any[];

  add(modal: any) {
    this.modalList.push(modal);
  }

  open(id: string) {
    for (const modal of this.modalList) {
      if (id === modal.id) {
        modal.openModal();
        return;
      }
    }
  }

  remove(id: string) {
    this.modalList.splice(this.modalList.indexOf(id), 1);
  }

  close(id: string) {
    for (const modal of this.modalList) {
      if (id === modal.id + "") {
        modal.closeModal();
        return;
      }
    }
  }

  constructor() {
    this.modalList = [];
  }
}
