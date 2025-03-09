import { Component, Input, OnInit, ViewChild,OnDestroy, ElementRef } from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import {ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { SliderComponent } from "../../utils/controls/slider/slider.component";
import { NumericComponent } from "../../utils/controls/numeric/numeric.component";
import { AudioPlayerComponent } from "../../utils/controls/audio-player/audio-player.component";
import { wav_preview_url } from '../../app/file_transfer/file_service/file-transfer.service';
import { Subscription, catchError, of } from 'rxjs';
import { AudioDTO, SoundDTO } from '../../models/models';
import { base_url,sound_actions } from '../../app/http';
import { sfx } from '../../assets/global_assets';
import { TranslateService,TranslatePipe, TranslateDirective} from '@ngx-translate/core';
import { HttpParams } from '@angular/common/http';
import { alert_localized } from '../../utils/alerts';
import { force_reload } from '../../app/http';
import { user_prefs } from '../../assets/user_prefs';
import { get_headers } from '../../utils/httpheaders';


@Component({
  selector: 'app-wl-form',
  standalone: true,
  imports: [ReactiveFormsModule, SliderComponent, NumericComponent, AudioPlayerComponent, TranslatePipe],
  templateUrl: './wl-form.component.html',
  styleUrl: './wl-form.component.css'
})
export class WlFormComponent {
  @Input() transfer!: FileTransferService;
  @ViewChild('player',{static:false})audioPlayer!:AudioPlayerComponent;
  private taskCompletedSubscription!: Subscription;
  form:FormGroup;

  public btn_enabled:boolean=true;
  public post_delete:boolean=true;
  public btn_translation: string = 'button.post';

  constructor(public translate: TranslateService){
    this.form = new FormGroup({

      t_numeric : new FormControl(10,[Validators.min(4),Validators.max(12),Validators.required]),
      t_slider : new FormControl(10,[Validators.min(4),Validators.max(12),Validators.required]),
      loop : new FormControl(false)

    });
  }


  ngOnInit(){
    this.taskCompletedSubscription = this.transfer.wavTaskCompleted$.subscribe(() => {
      this.post_delete=true;
      this.btn_translation='button.post';   
     this.audioPlayer.audio.nativeElement.src=wav_preview_url;
     this.audioPlayer.audio.nativeElement.load();
    });
    
  
    this.form.get('t_numeric')?.valueChanges.subscribe(value=>{
      if(value<4 || value===null){
        this.form.get('t_numeric')?.setValue(4,{emitEvent:false});
        this.form.get('t_slider')?.setValue(4,{emitEvent:false});
      }
      else if(value>12){
        this.form.get('t_numeric')?.setValue(12,{emitEvent:false});
        this.form.get('t_slider')?.setValue(12,{emitEvent:false});
      }
      else{
        this.form.get('t_slider')?.setValue(value,{emitEvent:false});
      }

    });


    this.form.get('t_slider')?.valueChanges.subscribe(value=>{
      if(value<4 || value===null){
        this.form.get('t_slider')?.setValue(4,{emitEvent:false});
        this.form.get('t_numeric')?.setValue(4,{emitEvent:false});
      }
      else if(value>12){
        this.form.get('t_slider')?.setValue(12,{emitEvent:false});
        this.form.get('t_numeric')?.setValue(12,{emitEvent:false});
      }
      else{
        this.form.get('t_numeric')?.setValue(value,{emitEvent:false});
      }

    });

   
  }

  ngOnDestroy(){
    this.taskCompletedSubscription.unsubscribe();
  }

    
  async create_sound_json() : Promise<SoundDTO>{   

    let thresholdB = this.form.get('t_numeric')?.value;
 
    let channelC = this.transfer.accessBinaryFile(22,2) as number;

    let looping = this.form.get('loop')?.value;

    const $instance = await SoundDTO.create(this.transfer.file_text!,thresholdB,(channelC==null)?1:channelC, looping);

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

  }

  async post():Promise<void>{

    const $result :SoundDTO= await this.create_sound_json();

    let $optimized = {...$result};
    $optimized.soundData=null;

    let full_url = `${base_url}${sound_actions}`;

    const $id :number |null = await this.transfer.generic_post($optimized,$result,full_url);

    if($id!=null){

      const $response : AudioDTO = await this.transfer.generic_get($id,full_url) as AudioDTO;

      sfx.reset($response.wl_ID,$response.audioData,$response.sampleRate,$response.channelCount,$response.blockCountPerChannel,$response.thresholdBits);

      this.transfer.wlTaskSource.next();

    }    

    

  }

}
