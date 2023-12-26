export interface Book {
    id?: number,
    name: string,
    rating: number,
    author: string,
    genre: string,
    isAvailable?: boolean,
    description:string,
    lentByUserId: string,
    borrowedByUserId ?:string
}
