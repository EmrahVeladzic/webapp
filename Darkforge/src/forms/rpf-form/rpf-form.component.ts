import { Component ,Input, OnInit} from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import { toggle_visibility } from '../../utils/dynamic_html';
import { ImageJson,TextureJson } from '../../models/models';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { base_url, image_actions } from '../../app/app.routes';
import { tex } from '../../assets/global_assets';


@Component({
  selector: 'app-rpf-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './rpf-form.component.html',
  styleUrl: './rpf-form.component.css'
})
export class RpfFormComponent implements OnInit{
  @Input() transfer!: FileTransferService;
  form :FormGroup;
 


  constructor(){
    this.form = new FormGroup({

      clut: new FormControl(256,[Validators.min(2),Validators.max(256)]),
      mode: new FormControl('0'),
      use_alpha: new FormControl(false),
      r_slider : new FormControl(0,[Validators.min(0),Validators.max(255)]),
      g_slider : new FormControl(0,[Validators.min(0),Validators.max(255)]),
      b_slider : new FormControl(0,[Validators.min(0),Validators.max(255)]),
      r_numeric : new FormControl(0,[Validators.min(0),Validators.max(255)]),
      g_numeric : new FormControl(0,[Validators.min(0),Validators.max(255)]),
      b_numeric : new FormControl(0,[Validators.min(0),Validators.max(255)]),
      bfr : new FormControl(0,[Validators.min(0),Validators.max(4)])


    });
  }

  ngOnInit(){
    this.form.get('clut')?.valueChanges.subscribe(value=>{
      if(value<2){
       this.form.get('clut')?.setValue(2,{emitEvent:false});
      }
      else if (value>256){
        this.form.get('clut')?.setValue(256,{emitEvent:false});
      }

      console.log(this.form.get('mode')?.value);
    }); 

    this.form.get('r_slider')?.valueChanges.subscribe(value=>{
      if(value<0){
        this.form.get('r_slider')?.setValue(0,{emitEvent:false});
        this.form.get('r_numeric')?.setValue(0,{emitEvent:false});
      }
      else if(value>255){
        this.form.get('r_slider')?.setValue(255,{emitEvent:false});
        this.form.get('r_numeric')?.setValue(255,{emitEvent:false});
      }
      else{
        this.form.get('r_numeric')?.setValue(value,{emitEvent:false});
      }

    });

    this.form.get('g_slider')?.valueChanges.subscribe(value=>{
      if(value<0){
        this.form.get('g_slider')?.setValue(0,{emitEvent:false});
        this.form.get('g_numeric')?.setValue(0,{emitEvent:false});
      }
      else if(value>255){
        this.form.get('g_slider')?.setValue(255,{emitEvent:false});
        this.form.get('g_numeric')?.setValue(255,{emitEvent:false});
      }
      else{
        this.form.get('g_numeric')?.setValue(value,{emitEvent:false});
      }

    });
    
    this.form.get('b_slider')?.valueChanges.subscribe(value=>{
      if(value<0){
        this.form.get('b_slider')?.setValue(0,{emitEvent:false});
        this.form.get('b_numeric')?.setValue(0,{emitEvent:false});
      }
      else if(value>255){
        this.form.get('b_slider')?.setValue(255,{emitEvent:false});
        this.form.get('b_numeric')?.setValue(255,{emitEvent:false});
      }
      else{
        this.form.get('b_numeric')?.setValue(value,{emitEvent:false});
      }

    });


    this.form.get('r_numeric')?.valueChanges.subscribe(value=>{
      if(value<0){
        this.form.get('r_numeric')?.setValue(0,{emitEvent:false});
        this.form.get('r_slider')?.setValue(0,{emitEvent:false});
      }
      else if(value>255){
        this.form.get('r_numeric')?.setValue(255,{emitEvent:false});
        this.form.get('r_slider')?.setValue(255,{emitEvent:false});
      }
      else{
        this.form.get('r_slider')?.setValue(value,{emitEvent:false});
      }

    });

    this.form.get('g_numeric')?.valueChanges.subscribe(value=>{
      if(value<0){
        this.form.get('g_numeric')?.setValue(0,{emitEvent:false});
        this.form.get('g_slider')?.setValue(0,{emitEvent:false});
      }
      else if(value>255){
        this.form.get('g_numeric')?.setValue(255,{emitEvent:false});
        this.form.get('g_slider')?.setValue(255,{emitEvent:false});
      }
      else{
        this.form.get('g_slider')?.setValue(value,{emitEvent:false});
      }

    });
    
    this.form.get('b_numeric')?.valueChanges.subscribe(value=>{
      if(value<0){
        this.form.get('b_numeric')?.setValue(0,{emitEvent:false});
        this.form.get('b_slider')?.setValue(0,{emitEvent:false});
      }
      else if(value>255){
        this.form.get('b_numeric')?.setValue(255,{emitEvent:false});
        this.form.get('b_slider')?.setValue(255,{emitEvent:false});
      }
      else{
        this.form.get('b_slider')?.setValue(value,{emitEvent:false});
      }

    });

    this.form.get('bfr')?.valueChanges.subscribe(value=>{
      if(value<0){
       this.form.get('bfr')?.setValue(0,{emitEvent:false});
      }
      else if (value>4){
        this.form.get('bfr')?.setValue(4,{emitEvent:false});
      }
    });

  }

  getRGBColor(): string {
    const r = this.form.get('r_numeric')?.value;
    const g = this.form.get('g_numeric')?.value;
    const b = this.form.get('b_numeric')?.value;
    return `rgb(${r}, ${g}, ${b})`;
  } 
   
   
  async create_image_json() : Promise<ImageJson>{   

    

    let CLUT_size = this.form.get('clut')?.value;
    
    let r_out = this.form.get('r_numeric')?.value;
    let g_out = this.form.get('g_numeric')?.value;
    let b_out = this.form.get('b_numeric')?.value;
   
    let mode_slc = this.form.get('mode')?.value==='1';

    let BFR_size = this.form.get('bfr')?.value;

    let CHK = this.form.get('use_alpha')?.value;

    const $instance = await ImageJson.create(this.transfer.img_text!,parseInt(CLUT_size),(CHK)?[parseInt(r_out),parseInt(g_out),parseInt(b_out)]:null,(mode_slc),parseInt(BFR_size));

    return $instance;

  }



  post_image($event : Event){

   (this.create_image_json()).then($result=>{



    let post_url = `${base_url}/${image_actions}`;


    this.transfer.http.post(post_url,$result).subscribe($response=>{

      let TextureResponse = $response as TextureJson;
      
      tex.reset(TextureResponse.clut,TextureResponse.pixels,(TextureResponse.width+1),(TextureResponse.height+1));
    

    });
  



    });



  }

 

}
