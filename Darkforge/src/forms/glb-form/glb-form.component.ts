import { Component ,Input} from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { NumericComponent } from "../../utils/controls/numeric/numeric.component";


@Component({
  selector: 'app-glb-form',
  standalone: true,
  imports: [ReactiveFormsModule, NumericComponent],
  templateUrl: './glb-form.component.html',
  styleUrl: './glb-form.component.css'
})
export class GlbFormComponent {
 @Input() transfer!: FileTransferService;
 form :FormGroup;

constructor(){
 
  this.form=new FormGroup({

    
  });







}



}
