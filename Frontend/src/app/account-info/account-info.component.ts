import { Component, OnInit } from '@angular/core';
import { AccountInfo, AccountService } from '../account.service';
import { AsyncPipe, NgIf } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-account-info',
  imports: [AsyncPipe, RouterLink, RouterLinkActive, NgIf],
  templateUrl: './account-info.component.html',
  styleUrl: './account-info.component.css'
})
export class AccountInfoComponent implements OnInit {
  info$!: Observable<AccountInfo>
  logOutSuccess: boolean = false;
  router: Router

  constructor(private accountService: AccountService, private cookieService: CookieService, private constructorRouter: Router){
    this.router = constructorRouter;
  }

  ngOnInit(): void {
    this.info$ = this.accountService.accountInfo();
  }
     
  logOut() {
    this.cookieService.deleteAll();
    this.logOutSuccess = true;
  }
}