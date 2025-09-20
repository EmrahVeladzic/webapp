import { Component ,Input, OnInit, OnDestroy} from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { NumericComponent } from "../../utils/controls/numeric/numeric.component";
import { base_url, model_actions } from '../../app/http';
import { AssetDTO, ModelDTO } from '../../models/models';
import { Asset } from '../../app/renderer/formats';
import { flip_ast_state,ast, update_anim } from '../../assets/global_assets';
import { TranslateService,TranslatePipe, TranslateDirective} from '@ngx-translate/core';
import { Subscription,catchError, of } from 'rxjs';


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
  private taskCompletedSubscription!: Subscription;
 public btn_enabled:boolean=true;
 public post_delete:boolean=true;
 public btn_translation: string = 'button.post';

constructor(public translate: TranslateService){
 
  this.form=new FormGroup({
    precision:new FormControl('12'),
    fps:new FormControl('60'),
    tex_x : new FormControl(128,[Validators.min(16),Validators.max(256),Validators.required]),
    tex_y : new FormControl(128,[Validators.min(16),Validators.max(256),Validators.required]),
    
    
  });



}

ngOnDestroy() {
  this.taskCompletedSubscription.unsubscribe();
}

ngOnInit(){

  this.taskCompletedSubscription = this.transfer.glbTaskCompleted$.subscribe(() => {
    this.post_delete=true;
    this.btn_translation='button.post';   
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
 


