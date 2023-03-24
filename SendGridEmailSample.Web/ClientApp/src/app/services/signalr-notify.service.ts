import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { EmailAlertModel } from '../models/email-alert.model';

@Injectable({
  providedIn: 'root',
})
export class SignalrNotifyService {
  private _hubConnection: signalR.HubConnection;
  private _statusUpdatedSubject = new Subject<EmailAlertModel>();
  statusUpdatedSubject$ = this._statusUpdatedSubject.asObservable();

  constructor() {
    this._hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.baseUrl}/email-status-change-event-hub`)
      .build();
  }

  startConnection() {
    this._hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));
  }

  listenOnStatusUpdated(): void {
    this._hubConnection.on('StatusUpdated', (message: EmailAlertModel) => {
      this._statusUpdatedSubject.next(message);
    });
  }
}
