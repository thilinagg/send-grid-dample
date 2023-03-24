import { Component, OnInit } from '@angular/core';
import { MdbModalRef, MdbModalService } from 'mdb-angular-ui-kit/modal';
import { EmailAlertService } from 'src/app/services/email-alert.service';
import { SendEmailComponent } from '../send-email/send-email.component';

@Component({
  selector: 'app-header-nav',
  templateUrl: './header-nav.component.html',
  styleUrls: ['./header-nav.component.scss']
})
export class HeaderNavComponent implements OnInit {

  modalRef: MdbModalRef<SendEmailComponent> | null = null;

  constructor(private modalService: MdbModalService, private emailAlertService: EmailAlertService,) { }

  ngOnInit() {
    this.emailAlertService.closeSendEmailModal$.subscribe({
      next: () => {
        this.modalRef?.close();
      }
    })
  }

  openModal() {
    this.modalRef = this.modalService.open(SendEmailComponent, { ignoreBackdropClick: true});
  }
}
