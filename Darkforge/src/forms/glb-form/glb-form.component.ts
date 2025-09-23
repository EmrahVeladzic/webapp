import { Component ,Input, OnInit, OnDestroy, ViewChild} from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { NumericComponent } from "../../utils/controls/numeric/numeric.component";
import { base_url, model_actions } from '../../app/http';
import { AssetDTO, ModelDTO } from '../../models/models';
import { Asset } from '../../app/renderer/formats';
import { flip_ast_state,ast, update_anim, tex } from '../../assets/global_assets';
import { TranslateService,TranslatePipe, TranslateDirective} from '@ngx-translate/core';
import { Subject, takeUntil, Subscription,catchError, of } from 'rxjs';


@Component({
  selector: 'app-glb-form',
  standalone: true,
  imports: [ReactiveFormsModule, NumericComponent, TranslatePipe],
  templateUrl: './glb-form.component.html',
  styleUrl: './glb-form.component.css'
})
export class GlbFormComponent {
 @Input() transfer!: FileTransferService;
 form :FormGroup;

  @ViewChild('width')t_width!:NumericComponent;
  @ViewChild('height')t_height!:NumericComponent;
  @ViewChild('tpx')t_page_x!:NumericComponent;
  @ViewChild('tpy')t_page_y!:NumericComponent;
  @ViewChild('tox')t_offset_x!:NumericComponent;
  @ViewChild('toy')t_offset_y!:NumericComponent;


 private destroy$ = new Subject<void>();

 public btn_enabled:boolean=true;
 public post_delete:boolean=true;
 public btn_translation: string = 'button.post';

constructor(public translate: TranslateService){
 
  this.form=new FormGroup({
    precision:new FormControl('12'),
    fps:new FormControl('60'),
    tex_x : new FormControl(128,[Validators.min(16),Validators.max(256),Validators.required]),
    tex_y : new FormControl(128,[Validators.min(16),Validators.max(256),Validators.required]),
    tex_bpp : new FormControl(8,[Validators.min(4),Validators.max(8),Validators.required]),
    tex_p_x : new FormControl(5,[Validators.min(5),Validators.max(15),Validators.required]),
    tex_p_y : new FormControl(0,[Validators.min(0),Validators.max(1),Validators.required]),
    tex_o_x : new FormControl(0,[Validators.min(0),Validators.max(15),Validators.required]),
    tex_o_y : new FormControl(0,[Validators.min(0),Validators.max(15),Validators.required]),
    
  });



}

recalculate_texture_params():void{

  let divisor = 16/this.form.get('tex_bpp')?.value;
  let width = this.t_width!.getValue();
  let height = this.t_height!.getValue();

  let max_tp_x = (1024-(width/divisor))/64;
  let max_to_x = (max_tp_x-Math.trunc(max_tp_x))*16;

  max_tp_x = Math.trunc(max_tp_x);

  let max_to_y = (256-height)/16;

  this.t_page_x.max = max_tp_x;
  this.t_offset_x.max = max_to_x;
  this.t_offset_y.max = max_to_y;

  this.t_page_x.writeValue(Math.min(this.t_page_x.getValue(),max_tp_x));
  this.t_offset_x.writeValue(Math.min(this.t_offset_x.getValue(),max_to_x));
  this.t_offset_y.writeValue(Math.min(this.t_offset_y.getValue(),max_to_y));

}




ngOnDestroy() {
  this.destroy$.next();
  this.destroy$.complete();
}

ngOnInit(){

  this.transfer.glbTaskCompleted$.pipe(takeUntil(this.destroy$)).subscribe(() => {
    this.post_delete=true;
    this.btn_translation='button.post';   

    if(tex!=null){
      this.t_width!.writeValue(tex.Width);
      this.t_height!.writeValue(tex.Height);
      this.form.get('tex_bpp')?.setValue((tex.CLUT.length>16)?8:4);
      this.t_page_x!.writeValue(tex.texturePage_X);
      this.t_page_y!.writeValue(tex.texturePage_Y);
      this.t_offset_x!.writeValue(tex.textureOffset_X);
      this.t_offset_y!.writeValue(tex.textureOffset_Y);
      this.recalculate_texture_params();
    }



  });


  this.form.get('tex_bpp')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(value=>{
    this.recalculate_texture_params();
  });
  
  this.form.get('tex_x')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(value=>{
    this.recalculate_texture_params();
  });

  this.form.get('tex_y')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(value=>{
    this.recalculate_texture_params();
  });

}



  async create_model_json() : Promise<ModelDTO>{   

    
  let precision_bits = this.form.get('precision')?.value;

  let framerate = this.form.get('fps')?.value;

  let tex_width = this.form.get('tex_x')?.value;

  let tex_height = this.form.get('tex_y')?.value;

  const $instance = await ModelDTO.create(this.transfer.file_text!,precision_bits,framerate,tex_width,tex_height);

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

    const full_url = `${base_url}${model_actions}`;

    await this.transfer.generic_delete(ast.id,full_url);
    
    this.transfer.reset_ast();



  }

  async post():Promise<void>{

    const $result :ModelDTO= await this.create_model_json();

    let $optimized = {...$result};
    $optimized.modelData=null;

    const full_url = `${base_url}${model_actions}`;

    const $id :number |null = await this.transfer.generic_post($optimized,$result,full_url);

    if($id!=null){

      const $response : AssetDTO = await this.transfer.generic_get($id,full_url) as AssetDTO;

      update_anim(null);
      flip_ast_state();

      ast.reset($response.asT_ID,$response.asset.precisionBits,$response.asset.mdl??null,$response.asset.fkr??null)

      flip_ast_state();
    }    

    

  }


}
 


