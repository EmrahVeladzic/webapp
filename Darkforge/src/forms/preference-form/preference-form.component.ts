import { Component ,Input} from '@angular/core';
import { FileTransferService } from '../../app/file_transfer/file_service/file-transfer.service';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { user_prefs , UserPreferences, set_prefs, pref$} from '../../assets/user_prefs';
import { base_url,pref_actions } from '../../app/http';
import { TranslateService,TranslatePipe, TranslateDirective} from '@ngx-translate/core';
import { Subscription, take } from 'rxjs';

@Component({
  selector: 'app-preference-form',
  standalone: true,
  imports: [ReactiveFormsModule, TranslatePipe],
  templateUrl: './preference-form.component.html',
  styleUrl: './preference-form.component.css'
})
export class PreferenceFormComponent {
  @Input() transfer!: FileTransferService;
  form :FormGroup;
  public btn_enabled:boolean=true;
  public pref_subscription!:Subscription;

 

  constructor(public translate: TranslateService){

    this.form=new FormGroup({
      lang:new FormControl(user_prefs.language),
      share:new FormControl(user_prefs.shareAssetOwnership)   
    });

    this.pref_subscription=pref$.pipe(take(1)).subscribe(()=>{
       
      this.form.get('lang')?.setValue(user_prefs.language);
      this.form.get('share')?.setValue(user_prefs.shareAssetOwnership);

    });
  }


  async patch($event :Event):Promise<void>{

    let prefs = {...user_prefs};
    prefs.language = this.form.get('lang')?.value;
    prefs.shareAssetOwnership=this.form.get('share')?.value;

    const full_url = `${base_url}${pref_actions}`;

    await this.transfer.generic_put(prefs,full_url);

    set_prefs(prefs);

    this.translate.use(user_prefs.language);

  }



}
