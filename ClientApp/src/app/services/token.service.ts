import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  
  private url = 'https://localhost:5001/account';
  private tokenCountChange = new BehaviorSubject<number>(0);
  constructor(private http:HttpClient) { }

  getTokenCount(userEmail: string):Observable<number>{
    return this.http.get<number>(`${this.url}/tokens?userEmail=${userEmail}`);
  }
  getTokenCountChange(): BehaviorSubject<number> {
    return this.tokenCountChange;
  }
  updateTokenCount(userEmail:string){
    this.getTokenCount(userEmail).subscribe({
      next: res =>{
        this.tokenCountChange.next(res);
      },
      error: err => console.error(err)
    })

  }
}
