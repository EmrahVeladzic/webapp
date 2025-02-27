import { Component ,Input, OnInit, ViewChild, OnDestroy, ElementRef} from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import { ImageJson,TextureJson } from '../../models/models';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { base_url, image_actions } from '../../app/app.routes';
import { tex } from '../../assets/global_assets';
import { SliderComponent } from "../../utils/controls/slider/slider.component";
import { NumericComponent } from '../../utils/controls/numeric/numeric.component';
import { bmp_preview_url } from '../../app/file_transfer/file_service/file-transfer.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-rpf-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, SliderComponent,NumericComponent],
  templateUrl: './rpf-form.component.html',
  styleUrl: './rpf-form.component.css'
})
export class RpfFormComponent implements OnInit{
  @Input() transfer!: FileTransferService;
  form :FormGroup;
  @ViewChild('bmp_preview',{static:false})cnv!:ElementRef<HTMLCanvasElement>;
  private ctx? : CanvasRenderingContext2D;
  private preview? : HTMLImageElement;
  private taskCompletedSubscription!: Subscription;
 
  @ViewChild('r_s',{static:false})r_s!:SliderComponent;
  @ViewChild('g_s',{static:false})g_s!:SliderComponent;
  @ViewChild('b_s',{static:false})b_s!:SliderComponent;

  constructor(){
    this.form = new FormGroup({

      clut: new FormControl(16,[Validators.min(2),Validators.max(256),Validators.required]),
      mode: new FormControl('0'),
      use_alpha: new FormControl(false),
      r_slider : new FormControl(0,[Validators.min(0),Validators.max(255),Validators.required]),
      g_slider : new FormControl(0,[Validators.min(0),Validators.max(255),Validators.required]),
      b_slider : new FormControl(0,[Validators.min(0),Validators.max(255),Validators.required]),
      r_numeric : new FormControl(0,[Validators.min(0),Validators.max(255),Validators.required]),
      g_numeric : new FormControl(0,[Validators.min(0),Validators.max(255),Validators.required]),
      b_numeric : new FormControl(0,[Validators.min(0),Validators.max(255),Validators.required]),
      bfr : new FormControl(0,[Validators.min(0),Validators.max(2),Validators.required])


    });
  }

  draw_preview(){
  
 
    this.ctx = this.cnv.nativeElement.getContext("2d") as CanvasRenderingContext2D;
    this.ctx!.imageSmoothingEnabled=false;

    
    this.preview! = new Image();

    this.preview!.src=bmp_preview_url;

    this.preview!.onload = () =>{
      
        
      this.ctx?.drawImage(this.preview!,0,0,this.cnv!.nativeElement.width,this.cnv!.nativeElement.height);

      
    }
  
  }

  ngOnDestroy() {
    this.taskCompletedSubscription.unsubscribe();
  }

  ngOnInit(){
    this.taskCompletedSubscription = this.transfer.bmpTaskCompleted$.subscribe(() => {
      this.draw_preview();
    });

    this.form.get('clut')?.valueChanges.subscribe(value=>{
      if(value<2){
       this.form.get('clut')?.setValue(2,{emitEvent:false});
      }
      else if (value>256 || value===null){
        this.form.get('clut')?.setValue(256,{emitEvent:false});
      }

    }); 

    this.form.get('use_alpha')?.valueChanges.subscribe(value=>{
      if(value){
        this.r_s.sliderDimensionReset();
        this.g_s.sliderDimensionReset();
        this.b_s.sliderDimensionReset();
      }

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
      if(value<0 || value===null){
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
      if(value<0 || value===null){
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
      if(value<0 || value===null){
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
      if(value<0 || value===null){
       this.form.get('bfr')?.setValue(0,{emitEvent:false});
      }
      else if (value>2){
        this.form.get('bfr')?.setValue(2,{emitEvent:false});
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

    const $instance = await ImageJson.create(this.transfer.file_text!,parseInt(CLUT_size),(CHK)?[parseInt(r_out),parseInt(g_out),parseInt(b_out)]:null,(mode_slc),parseInt(BFR_size));

    return $instance;

  }



  post_image($event : Event):void{

   (this.create_image_json()).then($result=>{



    let post_url = `${base_url}/${image_actions}`;


    this.transfer.http.post(post_url,$result,{observe:"response"}).subscribe($response=>{
   
      if($response.status===200){

        let TextureResponse = $response.body as TextureJson;
      
        tex.reset(TextureResponse.clut,TextureResponse.pixels,(TextureResponse.width+1),(TextureResponse.height+1));      
  

      }

      
    });
  



    });



  }

 

}
