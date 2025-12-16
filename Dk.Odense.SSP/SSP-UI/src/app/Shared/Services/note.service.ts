import { Injectable } from '@angular/core';
import { BaseService } from '@services/base.service';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Note } from '@domain-models/note.model';

@Injectable({
  providedIn: 'root',
})
export class NoteService extends BaseService<Note> {
  constructor(http: HttpClient) {
    super(http);
    this.endpoint = 'noteshared';
  }

  getNotesByPerson(id: string): Observable<Note[]> {
    return this.getHTTPService().get<Note[]>(
      this.baseUrl + this.endpoint + '/GetNoteByPerson?personId=' + id
    );
  }

  saveNoteToPerson(note: Note): Observable<Note> {
    return this.post(note);
  }
}
