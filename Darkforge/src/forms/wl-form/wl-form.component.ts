import { Component, Input, OnInit, ViewChild,OnDestroy, ElementRef } from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SliderComponent } from "../../utils/controls/slider/slider.component";
import { NumericComponent } from "../../utils/controls/numeric/numeric.component";
import { AudioPlayerComponent } from "../../utils/controls/audio-player/audio-player.component";
import { wav_preview_url } from '../../app/file_transfer/file_service/file-transfer.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-wl-form',
  standalone: true,
  imports: [SliderComponent, NumericComponent, AudioPlayerComponent],
  templateUrl: './wl-form.component.html',
  styleUrl: './wl-form.component.css'
})
export class WlFormComponent {
  @Input() transfer!: FileTransferService;
  @ViewChild('player',{static:false})audioPlayer!:AudioPlayerComponent;
  private taskCompletedSubscription!: Subscription;

  ngOnInit(){
    this.taskCompletedSubscription = this.transfer.wavTaskCompleted$.subscribe(() => {
     this.audioPlayer.audio.nativeElement.src=wav_preview_url;
     this.audioPlayer.audio.nativeElement.load();
    });
  }

  ngOnDestroy(){
    this.taskCompletedSubscription.unsubscribe();
  }



}
