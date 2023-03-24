import { Component, OnInit } from '@angular/core';
import { SignalrNotifyService } from './services/signalr-notify.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'ClientApp';

  constructor(public signalrNotifyService: SignalrNotifyService) {}

  ngOnInit() {
    this.signalrNotifyService.startConnection();
    this.signalrNotifyService.listenOnStatusUpdated();
  }
}
