import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Book } from '../models/book.model';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class BooksService {

  constructor(private http: HttpClient, private authService: AuthService, private router: Router) { }
  url = "https://localhost:5001/books";

  getAllBooks():Observable<Book[]>{
    return this.http.get<Book[]>(`${this.url}/all`);
  }
  getAllLentedBooks(userEmail: string):Observable<Book[]>{
    return this.http.get<Book[]>(`${this.url}/all/lented?email=${userEmail}`);
  }
  getAllBorrowedBooks(userEmail:string):Observable<Book[]>{
    return this.http.get<Book[]>(`${this.url}/all/borrowed?email=${userEmail}`);
  }

  addBook(book: Book): Observable<Book>{
    return this.http.post<Book>(`${this.url}/add`,book);
  }

  getById(id:number): Observable<Book>{
    return this.http.get<Book>(`${this.url}?id=${id}`);
  }

  borrowBook(bookId:number, borrowerId:string):Observable<Book>{
    if(!this.authService.isAuthenticated()){
        this.router.navigate(['login']);
    }
    return this.http.put<Book>(`${this.url}/borrow?bookId=${bookId}&userid=${borrowerId}`,{})
  }

  returnBook(bookId:number): Observable<Book>{
    if(!this.authService.isAuthenticated()){
      this.router.navigate(['login']);
  }
    return this.http.put<Book>(`${this.url}/return?bookId=${bookId}`,{});
  }

}
