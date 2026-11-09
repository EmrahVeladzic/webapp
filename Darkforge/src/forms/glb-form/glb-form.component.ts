import { Component ,Input, OnInit, OnDestroy, ViewChild} from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
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
  imports: [ReactiveFormsModule, CommonModule, NumericComponent, TranslatePipe],
  templateUrl: './glb-form.component.html',
  styleUrl: './glb-form.component.css'
})
export class GlbFormComponent {
 @Input() transfer!: FileTransferService;
 form :FormGroup;

  @ViewChild('width')t_width!:NumericComponent;
  @ViewChild('height')t_height!:NumericComponent;
  @ViewChild('tox')t_offset_x!:NumericComponent;
  @ViewChild('toy')t_offset_y!:NumericComponent;


 private destroy$ = new Subject<void>();

 public btn_enabled:boolean=true;
 public post_delete:boolean=true;
 public btn_translation: string = 'button.post';

constructor(public translate: TranslateService){
 
  this.form=new FormGroup({
    precision:new FormControl('8'),
    fps:new FormControl('60'),
    tex_x : new FormControl(128,[Validators.min(16),Validators.max(256),Validators.required]),
    tex_y : new FormControl(128,[Validators.min(16),Validators.max(256),Validators.required]),
    tex_o_x : new FormControl(0,[Validators.min(0),Validators.max(240),Validators.required]),
    tex_o_y : new FormControl(0,[Validators.min(0),Validators.max(240),Validators.required]),
    use_rpf : new FormControl(false),
    rpf_id : new FormControl(0,[Validators.min(0),Validators.required]),

  });



}

private field_value(name:string):number{
  return Number(this.form.get(name)?.value) || 0;
}

private set_field(name:string, v:number){
  this.form.get(name)?.setValue(v,{emitEvent:false});
}

recalculate_texture_params():void{

  let width = this.field_value('tex_x');
  let height = this.field_value('tex_y');

  let max_to_x = Math.trunc((256-width)/32)*32;
  let max_to_y = 256-height;

  this.t_offset_x.max = max_to_x;
  this.t_offset_y.max = max_to_y;

  this.set_field('tex_o_x',Math.min(Math.trunc(this.field_value('tex_o_x')/32)*32,max_to_x));
  this.set_field('tex_o_y',Math.min(this.field_value('tex_o_y'),max_to_y));

}




apply_texture_placement():void{

  this.form.get('rpf_id')?.setValue(tex.id);

  if(this.form.get('use_rpf')?.value){
    return;
  }

  this.set_field('tex_x',tex.Width);
  this.set_field('tex_y',tex.Height);

  this.recalculate_texture_params();

  this.set_field('tex_o_x',tex.offsetX);
  this.set_field('tex_o_y',tex.offsetY);

}

ngOnDestroy() {
  this.destroy$.next();
  this.destroy$.complete();
}

ngOnInit(){

  this.transfer.glbTaskCompleted$.pipe(takeUntil(this.destroy$)).subscribe(() => {
    this.post_delete=true;
    this.btn_translation='button.post';   

    this.apply_texture_placement();

  });

  this.transfer.rpfTaskCompleted$.pipe(takeUntil(this.destroy$)).subscribe(() => {
    this.apply_texture_placement();
  });

  this.form.get('use_rpf')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(value=>{
    this.apply_texture_placement();
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

  let tox = this.form.get('tex_o_x')?.value;
  
  let toy = this.form.get('tex_o_y')?.value;

  let rpf = (this.form.get('use_rpf')?.value)?(Number(this.form.get('rpf_id')?.value)||0):0;

  const $instance = await ModelDTO.create(this.transfer.file_text!,precision_bits,framerate,tex_width,tex_height,tox,toy,rpf);

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