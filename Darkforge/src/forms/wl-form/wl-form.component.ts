import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SliderComponent } from "../../utils/controls/slider/slider.component";

@Component({
  selector: 'app-wl-form',
  standalone: true,
  imports: [SliderComponent],
  templateUrl: './wl-form.component.html',
  styleUrl: './wl-form.component.css'
})
export class WlFormComponent {
  @Input() transfer!: FileTransferService;

}
