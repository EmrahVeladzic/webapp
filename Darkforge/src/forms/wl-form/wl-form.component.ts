import { Component, Input, OnInit, ViewChild,OnDestroy, ElementRef } from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import {ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { SliderComponent } from "../../utils/controls/slider/slider.component";
import { NumericComponent } from "../../utils/controls/numeric/numeric.component";
import { AudioPlayerComponent } from "../../utils/controls/audio-player/audio-player.component";
import { wav_preview_url } from '../../app/file_transfer/file_service/file-transfer.service';
import { Subscription } from 'rxjs';
import { AudioJson, SoundJson } from '../../models/models';
import { base_url,sound_actions } from '../../app/app.routes';
import { sfx } from '../../assets/global_assets';

@Component({
  selector: 'app-wl-form',
  standalone: true,
  imports: [ReactiveFormsModule, SliderComponent, NumericComponent, AudioPlayerComponent],
  templateUrl: './wl-form.component.html',
  styleUrl: './wl-form.component.css'
})
export class WlFormComponent {
  @Input() transfer!: FileTransferService;
  @ViewChild('player',{static:false})audioPlayer!:AudioPlayerComponent;
  private taskCompletedSubscription!: Subscription;
  form:FormGroup;

  constructor(){
    this.form = new FormGroup({

      t_numeric : new FormControl(10,[Validators.min(4),Validators.max(12),Validators.required]),
      t_slider : new FormControl(10,[Validators.min(4),Validators.max(12),Validators.required]),
      loop : new FormControl(false)

    });
  }


  ngOnInit(){
    this.taskCompletedSubscription = this.transfer.wavTaskCompleted$.subscribe(() => {
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

    
  async create_sound_json() : Promise<SoundJson>{   

    let thresholdB = this.form.get('t_numeric')?.value;
 
    let channelC = this.transfer.accessBinaryFile(22,2);

    let looping = this.form.get('loop')?.value;

    const $instance = await SoundJson.create(this.transfer.file_text!,thresholdB,(channelC==null)?1:channelC, looping);

    return $instance;

  }

  post_audio($event :Event):void{

    (this.create_sound_json()).then($result=>{



      let post_url = `${base_url}/${sound_actions}`;
  
  
      this.transfer.http.post(post_url,$result).subscribe($response=>{
  
        let AudioResponse = $response as AudioJson;
        
        sfx.reset(AudioResponse.audioData,AudioResponse.sampleRate,AudioResponse.channelCount,AudioResponse.blockCountPerChannel,AudioResponse.thresholdBits);
      
        this.transfer.wlTaskSource.next();
  
      });
    
  
  
  
      });


  }


}
