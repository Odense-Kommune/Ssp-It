import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { VerifyWorryService } from '@services/verify-worry.service';

@Component({
  selector: 'app-worry-actions',
  templateUrl: './worry-actions.component.html',
  styleUrls: ['./worry-actions.component.scss'],
})
export class WorryActionsComponent implements OnInit {
  @Input() worryId!: string;

  @Output() pushRefresh = new EventEmitter<boolean>();

  constructor(private verifyWorryService: VerifyWorryService) {}

  unverify(id: string) {
    if (confirm('Vil du afverificere denne kriminalitetsbekymring?')) {
      this.verifyWorryService.unverify(id).subscribe((x) => {
        if (!x) {
          console.log('error');
        }
        this.pushRefresh.emit(true);
      });
    }

    this.pushRefresh.emit(true);
  }

  groundless(id: string) {
    if (confirm('Vil du afverificere denne kriminalitetsbekymring?')) {
      this.verifyWorryService.setGroundless(id).subscribe((x) => {
        if (!x) {
          console.log('error');
        }
        this.pushRefresh.emit(true);
      });
    }
  }

  ngOnInit() {}
}
