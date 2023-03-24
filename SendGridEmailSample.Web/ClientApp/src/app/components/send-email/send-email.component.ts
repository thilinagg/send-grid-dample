import { Component } from '@angular/core';
import { EmailAlertModel } from 'src/app/models/email-alert.model';
import { EmailAlertService } from 'src/app/services/email-alert.service';

@Component({
  selector: 'app-send-email',
  templateUrl: './send-email.component.html',
  styleUrls: ['./send-email.component.scss']
})
export class SendEmailComponent {
  emailAlert = new EmailAlertModel();
  isSaving = false;

  constructor(private emailAlertService: EmailAlertService) { }

  send() {
    if (this.emailAlert.receiverEmail == "" || this.emailAlert.subject == "" || this.emailAlert.body == "") {
      return;
    }

    if (this.isRecipientCountExeed()) {
      return;
    }

    this.isSaving = true;
    this.emailAlertService.send(this.emailAlert).subscribe({
      next: () => {
        this.isSaving = false;
      }
    })
  }

  isValied() {
    return this.emailAlert.receiverEmail != "" && this.emailAlert.subject != "" && this.emailAlert.body != "";
  }

  isRecipientCountExeed() {
    return this.emailAlert.receiverEmail.split(";")?.length > 5;
  }

  enableBulkEmail() {
    return this.emailAlert.isBulk == false && (this.emailAlert.receiverEmail.includes(',') || this.emailAlert.receiverEmail.includes(';'));
  }

  close() {
    this.emailAlertService.closeModal();
  }
}
