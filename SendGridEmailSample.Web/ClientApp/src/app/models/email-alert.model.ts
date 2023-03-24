import { EmailStatusEnum } from "../constants/email-status.enum";

export class EmailAlertModel {
  id: string = "";
  receiverEmail: string = "";
  subject: string = "";
  body: string = "";
  status: EmailStatusEnum = EmailStatusEnum.Processed;
  createdDateTime: Date = new Date();
  isBulk: boolean = false;
}
