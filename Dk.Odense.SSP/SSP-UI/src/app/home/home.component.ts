import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  days: string[];
  quotes: string[][];
  happy: string;

  constructor() {
    this.quotes = new Array(7);
    this.days = [
      'søndag',
      'mandag',
      'tirsdag',
      'onsdag',
      'torsdag',
      'fredag',
      'lørdag',
      'søndag',
    ];
    this.quotes[0] = [
      'saftig',
      'sanselig',
      'sej',
      'sjov',
      'selskabelig',
      'smuk',
    ];
    this.quotes[1] = [
      'magisk',
      'magnifik',
      'melodisk',
      'mesterlig',
      'mindeværdig',
      'morsom',
    ];
    this.quotes[2] = ['taknemmelig', 'tjenstlig', 'topprofessionel'];
    this.quotes[3] = [
      'opfindsom',
      'opkvikkende',
      'oplevelsesrig',
      'oprigtig',
      'optimistisk',
      'overraskende',
    ];
    this.quotes[4] = ['tjekket', 'tænksom', 'trivelig'];
    this.quotes[5] = [
      'frisk',
      'fancy',
      'fantasifuld',
      'fantastisk',
      'festlig',
      'formidabel',
    ];
    this.quotes[6] = ['laber', 'levende', 'livslysten', 'lykkelig'];
    const dayOfWeek = new Date().getDay();
    this.happy =
      "Ha' en " +
      this.quotes[dayOfWeek][
        Math.floor(Math.random() * this.quotes[dayOfWeek].length)
      ] +
      ' ' +
      this.days[dayOfWeek];
  }

  ngOnInit() {}
}
