import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { EmailAlertModel } from '../models/email-alert.model';

@Injectable({
  providedIn: 'root',
})
export class EmailAlertService {
  private readonly urlPrefix = `${environment.baseUrl}/api/EmailAlert`;
  private _closeSendEmailModal = new Subject<void>();
  closeSendEmailModal$ = this._closeSendEmailModal.asObservable();
  private _refreshList = new Subject<void>();
  refreshList$ = this._refreshList.asObservable();

  constructor(private http: HttpClient) { }

  getAllEmails() {
    return this.http.get<EmailAlertModel[]>(`${this.urlPrefix}/all`);
  }

  send(emailAlert: EmailAlertModel) {
    return this.http.post(`${this.urlPrefix}/send`, emailAlert).pipe(
      tap(() => {
        this._closeSendEmailModal.next();
        this._refreshList.next();
      })
    )
  }

  closeModal(){
    this._closeSendEmailModal.next();
  }
}
