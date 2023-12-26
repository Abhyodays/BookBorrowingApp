import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { TokenService } from 'src/app/services/token.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  token!:string|null;
  name?:string;
  tokenCount = 0;
  email?: string;

  constructor(private authService: AuthService,
    private tokenService: TokenService,
    private router: Router){}

  ngOnInit():void{
    if(this.authService.isTokenExpired()){
      this.authService.logout();
    }
    this.authService.getTokenChange().subscribe((token:string|null) =>{
      this.token = token;
      const decodedToken = this.authService.decodeToken(this.token);
      this.name = decodedToken.unique_name;
      this.email = decodedToken.email;
      if(this.email){
        this.tokenService.updateTokenCount(this.email!);
      }
    })
    if(this.email){
      this.tokenService.getTokenCount(this.email!).subscribe({
      next: res =>{
        this.tokenCount = res;
        // console.log("tokens: ", this.tokenCount);
      },
      error: err =>{
        console.error(err);
        
      }
    })
    }
    
    this.tokenService.getTokenCountChange().subscribe((count:number)=>{
      this.tokenCount = count;
    })
  }

  logout(){
    this.authService.logout();
    this.goToHome();
  }

  login(){
    this.router.navigate(["login"]);
  }
  isLoggedIn():boolean{
    return !!this.token;
  }

  goToHome(){
    this.router.navigate(['']);
  }

}
