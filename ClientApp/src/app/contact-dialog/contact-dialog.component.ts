import { Component, OnInit, Inject, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material';
import { NgxSpinnerService } from 'ngx-spinner';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';
import { EventEmitter } from 'events';
import { ModalData } from '../models/modal-data';
import { BehaviorSubject } from 'rxjs';
import { Contact } from '../models/contact';

@Component({
  selector: 'contact-dialog',
  templateUrl: './contact-dialog.component.html',
  styleUrls: ['./contact-dialog.component.css']
})
/** contact-dialog component*/
export class ContactDialogComponent implements OnInit {
  private readonly contactSaveUrl = "api/contact/";

  contactForm: FormGroup;
  //firstName() { return this.contactForm.get('firstName'); }
  //lastName() { return this.contactForm.get('lastName'); }
  //number() { return this.contactForm.get('number'); }
  //id() { return this.contactForm.get('id'); }

  constructor(private fb: FormBuilder, private http: HttpClient, private spinner: NgxSpinnerService, private dialogService: MatDialog, public dialogRef: MatDialogRef<ContactDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: any) {
  }

  ngOnInit() {
    this.contactForm = this.fb.group({
      firstName: new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(40),
        Validators.minLength(4)
      ])),
      lastName: new FormControl('', Validators.compose([
        Validators.maxLength(40)
      ])),
      number: new FormControl('', Validators.compose([
        Validators.required,
        Validators.pattern('^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$'),
      ])),
      id: new FormControl('')
    });
  }

  onContactSave(contactModel: Contact): void {
    this.spinner.show('requestSpinner');
    if (this.data.isNew) {
      contactModel.id = "00000000-0000-0000-0000-000000000000";
      this.http.post(this.contactSaveUrl, contactModel)
        .subscribe(response => {
          this.dialogRef.close();
          let modalData: ModalData = new ModalData(
            "Done",
            "Your new contact has been saved successfully.",
            "Okay",
            null);
          modalData.buttonPositiveHandler = () => {
            return;
          };
          this.dialogService.open(ConfirmationDialogComponent, {
            autoFocus: false,
            width: '400px',
            data: { header: "Done", message: 'Your new contact has been saved successfully.', buttonPositive: "Okay" }
          });
        }, error => {
          this.dialogRef.close();

          let modalData: ModalData = new ModalData(
            "Error",
            "An error occurred while adding your contact. Please try again later.",
            "Okay",
            null);
          modalData.buttonPositiveHandler = () => {
            return;
          };
          this.dialogService.open(ConfirmationDialogComponent, {
            autoFocus: false,
            width: '400px',
            data: modalData
          });
        });
    }
    else {
      this.http.put(this.contactSaveUrl, contactModel)
        .subscribe(response => {
          this.dialogRef.close();

          let modalData: ModalData = new ModalData(
            "Done",
            "Your contact has been edited successfully.",
            "Okay",
            null);
          modalData.buttonPositiveHandler = () => {
            return;
          };
          this.dialogService.open(ConfirmationDialogComponent, {
            autoFocus: false,
            width: '400px',
            data: modalData
          });
        }, error => {
          this.dialogRef.close();

          let modalData: ModalData = new ModalData(
            "Error",
            "An error occurred while saving your contact. Please try again later.",
            "Okay",
            null);
          modalData.buttonPositiveHandler = () => {
            return;
          };
          this.dialogService.open(ConfirmationDialogComponent, {
            autoFocus: false,
            width: '400px',
            data: modalData
          });
        });
    }
  }

  errorText(controlName: string): string {
    let control = this.contactForm.get(controlName);
    if (control.valid) {
      return "";
    }
    if (control.errors.maxlength) {
      return "Input too long";
    } else if (control.errors.minlength) {
      return "Input too short";
    } else if (control.errors.required) {
      return "Input required";
    } else {
      return "Invalid Input";
    }
  }

  length(control: string): number {
    return this.contactForm.get(control).value.length;
  }
}
