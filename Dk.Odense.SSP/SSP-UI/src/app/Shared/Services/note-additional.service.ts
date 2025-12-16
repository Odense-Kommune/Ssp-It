import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { NoteAdditional } from '@domain-models/note-additional.model';
import { BaseService } from '@services/base.service';

@Injectable({
  providedIn: 'root',
})
export class NoteAdditionalService extends BaseService<NoteAdditional> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'noteadditional';
  }

  getNotesByPerson(id: string): Observable<NoteAdditional[]> {
    return this.getHTTPService().get<NoteAdditional[]>(
      this.baseUrl + this.endpoint + '/GetNoteByPerson?personId=' + id
    );
  }

  saveNoteToPerson(note: NoteAdditional): Observable<NoteAdditional> {
    return this.post(note);
  }
}
