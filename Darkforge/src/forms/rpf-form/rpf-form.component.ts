import { Component ,Input, OnInit, ViewChild, OnDestroy, ElementRef} from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import { ImageDTO,TextureDTO } from '../../models/models';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { base_url, image_actions } from '../../app/http';
import { flip_tex_state, tex } from '../../assets/global_assets';
import { SliderComponent } from "../../utils/controls/slider/slider.component";
import { NumericComponent } from '../../utils/controls/numeric/numeric.component';
import { bmp_preview_url } from '../../app/file_transfer/file_service/file-transfer.service';
import { Subject,Subscription, catchError ,of, takeUntil} from 'rxjs';
import { alert_localized } from '../../utils/alerts';
import { force_reload } from '../../app/http';
import { TranslateService,TranslatePipe, TranslateDirective} from '@ngx-translate/core';
import { HttpParams } from '@angular/common/http';
import { user_prefs, UserPreferences } from '../../assets/user_prefs';
import { get_headers } from '../../utils/httpheaders';
import { link_slider_numeric } from '../../utils/dynamic_html';

@Component({
  selector: 'app-rpf-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, SliderComponent,NumericComponent, TranslatePipe],
  templateUrl: './rpf-form.component.html',
  styleUrl: './rpf-form.component.css'
})
export class RpfFormComponent implements OnInit{
  @Input() transfer!: FileTransferService;
  form :FormGroup;
  @ViewChild('bmp_preview',{static:false})cnv!:ElementRef<HTMLCanvasElement>;
  private ctx? : CanvasRenderingContext2D;
  private preview? : HTMLImageElement;


  private destroy$ = new Subject<void>();

  public btn_enabled:boolean=true;
  public post_delete:boolean=true;
  public btn_translation: string = 'button.post';
 
  @ViewChild('r_s',{static:false})r_s!:SliderComponent;
  @ViewChild('g_s',{static:false})g_s!:SliderComponent;
  @ViewChild('b_s',{static:false})b_s!:SliderComponent;

  @ViewChild('tpx',{static:false})tex_page_x!:SliderComponent;
  @ViewChild('tox',{static:false})tex_offset_x!:SliderComponent;
  @ViewChild('toy',{static:false})tex_offset_y!:SliderComponent;

  constructor(public translate: TranslateService){
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
      tex_p_x : new FormControl(5,[Validators.min(5),Validators.max(15),Validators.required]),
      tex_p_y : new FormControl(0,[Validators.min(0),Validators.max(1),Validators.required]),
      tex_o_x : new FormControl(0,[Validators.min(0),Validators.max(15),Validators.required]),
      tex_o_y : new FormControl(0,[Validators.min(0),Validators.max(15),Validators.required]),
    });
  }

  draw_preview(){
     
    this.ctx = this.cnv.nativeElement.getContext("2d") as CanvasRenderingContext2D;
    this.ctx!.imageSmoothingEnabled=false;

    
    this.preview = new Image();

    this.preview.src=bmp_preview_url;

    this.preview.onload = () =>{
      
      this.ctx?.drawImage(this.preview!,0,0,this.cnv!.nativeElement.width,this.cnv!.nativeElement.height);

      const value = this.form.get('clut')?.value;
      
      const divisor = (value>16)?2:4;
     
      let new_page_max = (1024-this.preview!.width/divisor)/64;    

      const new_offset_max = (new_page_max - Math.trunc(new_page_max))*16;

      new_page_max = Math.trunc(new_page_max);
     
      this.tex_page_x.max=new_page_max;
      this.tex_offset_x.max=new_offset_max;

      this.tex_page_x.writeValue(Math.min(this.tex_page_x.value,this.tex_page_x.max));
      this.tex_offset_x.writeValue(Math.min(this.tex_offset_x.value,this.tex_offset_x.max));

      this.tex_offset_y.max=(256-this.preview!.height)/16;
      this.tex_offset_y.writeValue(Math.min(this.tex_offset_y.value,this.tex_offset_y.max));
      
    }
  
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngOnInit(){
      this.transfer.bmpTaskCompleted$.pipe(takeUntil(this.destroy$)).subscribe(() => {
      this.post_delete=true;
      this.btn_translation='button.post';   
      this.draw_preview();
    });

    this.form.get('clut')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(value=>{
      
      const divisor = (value>16)?2:4;
     
      let new_page_max = (1024-this.preview!.width/divisor)/64;    

      const new_offset_max = (new_page_max - Math.trunc(new_page_max))*16;

      new_page_max = Math.trunc(new_page_max);
     
      this.tex_page_x.max=new_page_max;
      this.tex_offset_x.max=new_offset_max;

      this.tex_page_x.writeValue(Math.min(this.tex_page_x.value,this.tex_page_x.max));
      this.tex_offset_x.writeValue(Math.min(this.tex_offset_x.value,this.tex_offset_x.max));

    }); 

    this.form.get('use_alpha')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(value=>{
      if(value){
        this.r_s.sliderDimensionReset();
        this.g_s.sliderDimensionReset();
        this.b_s.sliderDimensionReset();
      }

    }); 

    link_slider_numeric(this.form,'r_slider','r_numeric',0,255,this.destroy$);
    link_slider_numeric(this.form,'g_slider','g_numeric',0,255,this.destroy$);
    link_slider_numeric(this.form,'b_slider','b_numeric',0,255,this.destroy$);

  }

  getRGBColor(): string {
    const r = this.form.get('r_numeric')?.value;
    const g = this.form.get('g_numeric')?.value;
    const b = this.form.get('b_numeric')?.value;
    return `rgb(${r}, ${g}, ${b})`;
  } 
   
   
  async create_image_json() : Promise<ImageDTO>{   

    

    let CLUT_size = this.form.get('clut')?.value;
    
    let r_out = this.form.get('r_numeric')?.value;
    let g_out = this.form.get('g_numeric')?.value;
    let b_out = this.form.get('b_numeric')?.value;
   
    let mode_slc = this.form.get('mode')?.value==='1';

    let tpx = this.form.get('tex_p_x')?.value;
    let tpy = this.form.get('tex_p_y')?.value;
    let tox = this.form.get('tex_o_x')?.value;
    let toy = this.form.get('tex_o_y')?.value;

    let CHK = this.form.get('use_alpha')?.value;

    const $instance = await ImageDTO.create(this.transfer.file_text!,parseInt(CLUT_size),(CHK)?[parseInt(r_out),parseInt(g_out),parseInt(b_out)]:null,(mode_slc),tpx,tpy,tox,toy);

    return $instance;

  }

  async choice($event :Event):Promise<void>{

    if(this.post_delete===true){
      this.btn_enabled = false;
      await this.post();      
      this.post_delete=false;
      this.btn_translation='button.delete';
      this.btn_enabled = true;
      
    }
    else{
      this.btn_enabled = false;
      await this.delete();
      this.post_delete=true;
      this.btn_translation='button.post';     
      this.btn_enabled = true;   
    }
  
  }

  
  async delete():Promise<void>{

    const full_url = `${base_url}${image_actions}`;

    await this.transfer.generic_delete(tex.id,full_url);
    
    this.transfer.reset_tex();


  }

  async post():Promise<void>{

    const $result :ImageDTO= await this.create_image_json();

    let $optimized = {...$result};
    $optimized.imageData=null;

    const full_url = `${base_url}${image_actions}`;

    const $id :number |null = await this.transfer.generic_post($optimized,$result,full_url);

    if($id!=null){

      const $response : TextureDTO = await this.transfer.generic_get($id,full_url) as TextureDTO;

      tex.reset($response.rpF_ID,$response.clut,$response.pixels,$response.width,$response.height,$response.texturePage_X,$response.texturePage_Y,$response.textureOffset_X,$response.textureOffset_Y);

      flip_tex_state();

    }    

    

  }

 

}
