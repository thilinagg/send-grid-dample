import { Component, OnInit } from '@angular/core';
import { EmailStatusEnum } from 'src/app/constants/email-status.enum';
import { EmailAlertModel } from 'src/app/models/email-alert.model';
import { EmailAlertService } from 'src/app/services/email-alert.service';
import { SignalrNotifyService } from 'src/app/services/signalr-notify.service';

@Component({
  selector: 'app-email-list',
  templateUrl: './email-list.component.html',
  styleUrls: ['./email-list.component.scss'],
})
export class EmailListComponent implements OnInit {
  emailList: EmailAlertModel[] = []
  EmailStatus = EmailStatusEnum;

  constructor(
    private emailAlertService: EmailAlertService,
    private signalrNotifyService: SignalrNotifyService
  ) { }

  ngOnInit() {
    this.getAllEmails();
    this.updateEmailStatus();
    this.refreshList();
  }

  getAllEmails() {
    this.emailAlertService.getAllEmails().subscribe({
      next: (res) => {
        this.emailList = res;
      },
    });
  }

  updateEmailStatus() {
    this.signalrNotifyService.statusUpdatedSubject$.subscribe({
      next: (data) => {
        this.emailList.forEach(elem => {
          if (elem.id == data.id)
            elem.status = data.status;
        })
      },
    });
  }

  refreshList() {
    this.emailAlertService.refreshList$.subscribe({
      next: () => {
        this.getAllEmails();
      }
    })
  }
}
