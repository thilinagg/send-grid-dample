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

  constructor(private emailAlertService: EmailAlertService){}

  send(){
    if(this.emailAlert.receiverEmail == "" || this.emailAlert.subject == "" || this.emailAlert.body == ""){
      return;
    }

    this.emailAlertService.send(this.emailAlert).subscribe({
      next: ()=>{

      }
    })
  }

  isValied(){
    return this.emailAlert.receiverEmail != "" && this.emailAlert.subject != "" && this.emailAlert.body != "";
  }

  close(){
    this.emailAlertService.closeModal();
  }
}
