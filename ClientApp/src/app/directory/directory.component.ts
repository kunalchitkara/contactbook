import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ModalData } from '../models/modal-data';
import { MatDialog, MatTableDataSource } from '@angular/material';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';
import { NgxSpinnerService } from "ngx-spinner";
import { SelectionModel } from '@angular/cdk/collections';
import { ContactDialogComponent } from '../contact-dialog/contact-dialog.component';
import { Contact } from '../models/contact';

@Component({
  selector: 'directory',
  templateUrl: './directory.component.html',
  styleUrls: ['./directory.component.css']
})
/** directory component*/
export class DirectoryComponent {
  /** directory ctor */
  constructor(
    private http: HttpClient,
    private dialogService: MatDialog,
    private spinner: NgxSpinnerService
  ) {
    this.bindContacts();
  }

  contactList: MatTableDataSource<Contact>;
  displayedColumns: string[] = ['select', 'name', 'number', 'actions'];
  selection = new SelectionModel<Contact>(true, []);

  private readonly newProperty = "api/contact/";

  bindContacts(keyword?: string): void {
    this.spinner.show();
    if (keyword == undefined)
      keyword = "";
    this.http.get(this.newProperty + keyword)
      .subscribe(
        contacts => {
          this.contactList = new MatTableDataSource<Contact>(contacts as Contact[]);
          this.spinner.hide();
        },
        error => {
          let modalData: ModalData = new ModalData(
            "Error",
            "An error occured while fetching data.",
            "Okay",
            null);

          this.dialogService.open(ConfirmationDialogComponent, {
            autoFocus: false,
            data: modalData
          });
          this.spinner.hide();
        }
      );
  }

  /** Whether the number of selected elements matches the total number of rows. */
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.contactList ? this.contactList.data.length : 0;
    return numSelected === numRows;
  }
  masterToggle() {
    this.contactList && this.isAllSelected() ?
      this.selection.clear() :
      this.contactList.data.forEach(row => this.selection.select(row));
  }

  checkboxLabel(row?: Contact): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.id}`;
  }

  applyFilter(filterValue: string) {
    this.contactList.filter = filterValue.trim().toLowerCase();
  }

  editContact(id: string) {
    let contactToEdit: Contact = this.contactList.data.filter(x => x.id == id)[0];
    let dialogRef = this.dialogService.open(ContactDialogComponent, {
      autoFocus: false,
      data: {
        isNew: false,
        contact: contactToEdit
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.bindContacts();
    });
  }

  addContact() {
    let dialogRef = this.dialogService.open(ContactDialogComponent, {
      autoFocus: false,
      data: {
        isNew: true,
        contact: {
          firstName: '',
          lastName: '',
          number: '',
          id: ''
        }
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.bindContacts();
    });
  }

  confirmDelete(id: string) {
    let modalData: ModalData = new ModalData(
      "CONFIRM DELETION",
      this.selection.selected.length > 1 ? "Are you sure you want to delete multiple contacts." : "Are you sure you want to delete this contact.",
      "Okay",
      null);
    modalData.buttonPositiveHandler = () => {
      if (this.selection.hasValue()) {
        this.deleteContact(id);
      }
      else {
        this.deleteContact(id);
      }
    };
    this.dialogService.open(ConfirmationDialogComponent, {
      autoFocus: false,
      data: modalData
    });
  }

  deleteContact(id: string) {
    this.spinner.show();
    if (this.selection.hasValue()) {
      this.http.delete("api/contact/" + this.selection.selected.map(x => x.id).join(','))
        .subscribe(
          isDeleted => {
            this.spinner.hide();
            if (isDeleted) {
              let modalData: ModalData = new ModalData(
                "Contact Deleted",
                "Requested contacts have been deleted successfully.",
                "Okay",
                null);
              modalData.buttonPositiveHandler = () => {
                this.bindContacts();
              };
              this.dialogService.open(ConfirmationDialogComponent, {
                autoFocus: false,
                data: modalData
              });
            } else {
              let modalData: ModalData = new ModalData(
                "Error",
                "An error occured while deleting contacts.",
                "Okay",
                null);
              this.dialogService.open(ConfirmationDialogComponent, {
                autoFocus: false,
                data: modalData
              });
            }
          },
          error => {
            this.spinner.hide();
            let modalData: ModalData = new ModalData(
              "Error",
              "An error occured while deleting contacts.",
              "Okay",
              null);
            this.dialogService.open(ConfirmationDialogComponent, {
              autoFocus: false,
              data: modalData
            });
          }
        );
    } else {
      this.http.delete("api/contact/" + id)
        .subscribe(
          contactId => {
            this.spinner.hide();
            let modalData: ModalData = new ModalData(
              "Contact Deleted",
              "Requested contact has been deleted successfully.",
              "Okay",
              null);
            modalData.buttonPositiveHandler = () => {
              this.bindContacts();
            };
            this.dialogService.open(ConfirmationDialogComponent, {
              autoFocus: false,
              data: modalData
            });
          },
          error => {
            this.spinner.hide();
            let modalData: ModalData = new ModalData(
              "Error",
              "An error occured while deleting contact.",
              "Okay",
              null);
            this.dialogService.open(ConfirmationDialogComponent, {
              autoFocus: false,
              data: modalData
            });
          }
        );
    }
  }
}
