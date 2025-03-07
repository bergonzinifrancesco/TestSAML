import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface ClaimInfo
{
  type: string
  value: string
}
export interface AccountInfo
{
  claims: ClaimInfo[]
  name: string | undefined
  authenticationType: string | undefined
}

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  constructor(private http: HttpClient) { }

  accountInfo(): Observable<AccountInfo> {
    return this.http.get<AccountInfo>("http://localhost:5220/account/info", { withCredentials: true, headers: {
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Credentials': 'true'
    }
  });
  }
}
