import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Book } from 'src/app/models/book.model';
import { AuthService } from 'src/app/services/auth.service';
import { BooksService } from 'src/app/services/books.service';
import { TokenService } from 'src/app/services/token.service';

@Component({
  selector: 'app-book-details',
  templateUrl: './book-details.component.html',
  styleUrls: ['./book-details.component.css']
})
export class BookDetailsComponent {
  book !: Book;
  userEmail!:string;
  tokenCount = 0;
  

  constructor(
    private route: ActivatedRoute,
    private booksServices: BooksService,
    private authService: AuthService,
    private tokenService: TokenService
  ){}

  ngOnInit(){
    this.loadBook();
    this.authService.getTokenChange().subscribe((token:string|null) =>{
      const decodedToken = this.authService.decodeToken(token);
      this.userEmail = decodedToken.email;
    })
    if(this.userEmail ){
      this.tokenService.getTokenCount(this.userEmail).subscribe({
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
  loadBook(){
    const id = this.route.snapshot.params['id'];
    this.booksServices.getById(id).subscribe({
      next: res =>{
        this.book = res
      },
      error: err => console.error(err)
    })
  }

  disableBorrowBtn(){
    if(this.userEmail && this.book.lentByUserId == this.userEmail || !this.book.isAvailable || this.tokenCount<=0 ) return true;
    return false;
  }

  disableReturnBtn(){
    if(this.userEmail && this.book.borrowedByUserId == this.userEmail) return false;
    return true;
  }
  borrow(){
    this.booksServices.borrowBook(this.book.id!, this.userEmail).subscribe({
      next: res =>{
        // console.log("borrowed: ", res);
        this.tokenService.updateTokenCount(this.userEmail!);
        this.loadBook();
      },
      error: err => console.error(err)
    })
  }
  return(){
    this.booksServices.returnBook(this.book.id!).subscribe({
      next: res =>{
        // console.log("returned: ", res);
        this.loadBook();
      },
      error: err => console.error(err)
    })
  }
  hideBorrowBtn(){
    if(this.userEmail && this.book.borrowedByUserId == this.userEmail || this.book.lentByUserId == this.userEmail) return true;
    return false;
  }
  hideReturnBtn(){
    if(this.userEmail && this.book.lentByUserId == this.userEmail || !this.userEmail) return true;
  
    return false;
  }

}
