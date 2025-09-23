import { Component, Input, OnInit, ViewChild,OnDestroy, ElementRef } from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import {ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { SliderComponent } from "../../utils/controls/slider/slider.component";
import { NumericComponent } from "../../utils/controls/numeric/numeric.component";
import { AudioPlayerComponent } from "../../utils/controls/audio-player/audio-player.component";
import { wav_preview_url } from '../../app/file_transfer/file_service/file-transfer.service';
import { Subject,takeUntil, Subscription, catchError, of } from 'rxjs';
import { AudioDTO, SoundDTO } from '../../models/models';
import { base_url,sound_actions } from '../../app/http';
import { sfx } from '../../assets/global_assets';
import { TranslateService,TranslatePipe, TranslateDirective} from '@ngx-translate/core';
import { HttpParams } from '@angular/common/http';
import { alert_localized } from '../../utils/alerts';
import { force_reload } from '../../app/http';
import { user_prefs } from '../../assets/user_prefs';
import { get_headers } from '../../utils/httpheaders';
import { link_slider_numeric } from '../../utils/dynamic_html';

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
  form:FormGroup;

  public btn_enabled:boolean=true;
  public post_delete:boolean=true;
  public btn_translation: string = 'button.post';

  private destroy$ = new Subject<void>();

  constructor(public translate: TranslateService){
    this.form = new FormGroup({

      t_numeric : new FormControl(13,[Validators.min(8),Validators.max(16),Validators.required]),
      t_slider : new FormControl(13,[Validators.min(8),Validators.max(16),Validators.required]),
      loop : new FormControl(false)

    });
  }


  ngOnInit(){
    this.transfer.wavTaskCompleted$.pipe(takeUntil(this.destroy$)).subscribe(() => {
      this.post_delete=true;
      this.btn_translation='button.post';   
     this.audioPlayer.audio.nativeElement.src=wav_preview_url;
     this.audioPlayer.audio.nativeElement.load();
    });
    

    link_slider_numeric(this.form,'t_slider','t_numeric',8,16,this.destroy$);
  
   
  }

  ngOnDestroy(){
    this.destroy$.next();
    this.destroy$.complete();
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

    const full_url = `${base_url}${sound_actions}`;

    await this.transfer.generic_delete(sfx.id,full_url);
    
    this.transfer.reset_sfx();

  }

  async post():Promise<void>{

    const $result :SoundDTO= await this.create_sound_json();

    let $optimized = {...$result};
    $optimized.soundData=null;

    const full_url = `${base_url}${sound_actions}`;

    const $id :number |null = await this.transfer.generic_post($optimized,$result,full_url);

    if($id!=null){

      const $response : AudioDTO = await this.transfer.generic_get($id,full_url) as AudioDTO;    

      sfx.reset($response.wL_ID,$response.audioData,$response.sampleRate,$response.channelCount,$response.blockCountPerChannel);
    
      this.transfer.wlTaskSource.next();

    }    

    

  }

}
