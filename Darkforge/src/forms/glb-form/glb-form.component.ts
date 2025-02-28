import { Component ,Input} from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { NumericComponent } from "../../utils/controls/numeric/numeric.component";
import { base_url, model_actions } from '../../app/app.routes';
import { AssetJson, ModelJson } from '../../models/models';
import { Asset } from '../../app/renderer/formats';
import { flip_ast_state,ast, update_anim } from '../../assets/global_assets';


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
    precision:new FormControl('12'),
    fps:new FormControl('60'),
    tex_x : new FormControl(128,[Validators.min(2),Validators.max(256),Validators.required]),
    tex_y : new FormControl(128,[Validators.min(2),Validators.max(256),Validators.required]),
    
    
  });







}


 async create_model_json() : Promise<ModelJson>{   

    
  let precision_bits = this.form.get('precision')?.value;

  let framerate = this.form.get('fps')?.value;

  let tex_width = this.form.get('tex_x')?.value;

  let tex_height = this.form.get('tex_y')?.value;

  const $instance = await ModelJson.create(this.transfer.file_text!,precision_bits,framerate,tex_width,tex_height);

  return $instance;

  }

post_model($event : Event):void{

  (this.create_model_json()).then($result=>{



   let post_url = `${base_url}/${model_actions}`;


   this.transfer.http.post(post_url,$result,{observe:"response"}).subscribe($response=>{
  
    if($response.status===200){

      update_anim(null);

      let raw = $response.body as any;
      
      flip_ast_state();  
      ast.reset( raw.asset.id,
      raw.asset.precisionBits,
      raw.asset.mdl ?? null,
      raw.asset.fkr ?? null);

     
      flip_ast_state();
    
    }

     
   });
 



   });



 }


}
