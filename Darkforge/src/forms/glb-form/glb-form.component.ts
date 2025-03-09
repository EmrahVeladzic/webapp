import { Component ,Input} from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { NumericComponent } from "../../utils/controls/numeric/numeric.component";
import { base_url, model_actions } from '../../app/http';
import { AssetDTO, ModelDTO } from '../../models/models';
import { Asset } from '../../app/renderer/formats';
import { flip_ast_state,ast, update_anim } from '../../assets/global_assets';
import { TranslateService,TranslatePipe, TranslateDirective} from '@ngx-translate/core';
import { HttpParams } from '@angular/common/http';
import { catchError, of } from 'rxjs';
import { force_reload } from '../../app/http';
import { alert_localized } from '../../utils/alerts';
import { user_prefs } from '../../assets/user_prefs';
import { get_headers } from '../../utils/httpheaders';

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

constructor(public translate: TranslateService){
 
  this.form=new FormGroup({
    precision:new FormControl('12'),
    fps:new FormControl('60'),
    tex_x : new FormControl(128,[Validators.min(2),Validators.max(256),Validators.required]),
    tex_y : new FormControl(128,[Validators.min(2),Validators.max(256),Validators.required]),
    
    
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

post_model($event : Event):void{

  (this.create_model_json()).then($result=>{

    let $optimized = {...$result};
    $optimized.modelData=null;


   let full_url = `${base_url}${model_actions}`;


   this.transfer.http.post(full_url,$optimized,{headers:get_headers(),observe:"response"}).pipe(catchError($error=>{return of($error)})).subscribe($response=>{
  
    if($response.status>=400){

      if($response.status===401){
        alert_localized(this.translate,'alerts.timeout');
      }
      else{
        alert_localized(this.translate,'alerts.server_error');
      }
      
      force_reload();
    }



    else{

    if($response.status===200){

     

      this.transfer.http.get(full_url,{headers:get_headers(),observe:"response",params:new HttpParams().set('id',$response.body as number)}).pipe(catchError($error=>{return of($error)})).subscribe($response=>{  

        
        if($response.status>=400){

          if($response.status===401){
            alert_localized(this.translate,'alerts.timeout');
          }
          else{
            alert_localized(this.translate,'alerts.server_error');
          }
          
          force_reload();
        }
  

        else{

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
    }

    else if($response.status===204){

      this.transfer.http.post(full_url,$result,{headers:get_headers(),observe:"response"}).pipe(catchError($error=>{return of($error)})).subscribe($response=>{
  
                
        if($response.status>=400){

          if($response.status===401){
            alert_localized(this.translate,'alerts.timeout');
          }
          else{
            alert_localized(this.translate,'alerts.server_error');
          }
          
          force_reload();
        }
  

        else{

        this.transfer.http.get(full_url,{headers:get_headers(),observe:"response",params:new HttpParams().set('id',$response.body as number)}).pipe(catchError($error=>{return of($error)})).subscribe($response=>{  
    
          
          if($response.status>=400){

            if($response.status===401){
              alert_localized(this.translate,'alerts.timeout');
            }
            else{
              alert_localized(this.translate,'alerts.server_error');
            }
            
            force_reload();
          }
    

          else{

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
        
        }
         
       });

      
    }
  } 

   });
 



   });



 }


}
