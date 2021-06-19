import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Settings } from 'app/shared/models/settings.model';
import { environment } from 'environments/environment';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

  constructor(private http: HttpClient) { }

  getLastSetting():Observable<Settings>{
    let ret:Settings= new Settings(); 
    return of(ret);
  }

  updateSettings(settings:Settings):Observable<Settings>{
    let ret:Settings= new Settings(); 
    return of(ret);
  }

  resetSettings():Observable<{}>{
    let ret:Settings= new Settings(); 
    return of({});
  }
}
