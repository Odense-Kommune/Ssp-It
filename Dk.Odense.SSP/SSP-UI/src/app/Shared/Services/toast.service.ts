import { AppComponent } from '../../app.component';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private appComponent: AppComponent | undefined;

  registerComponent(appComponent: any) {
    this.appComponent = appComponent;
  }

  createToast(message: string, type: string) {
    // this.appComponent.createComponent(message, type);
  }
}
