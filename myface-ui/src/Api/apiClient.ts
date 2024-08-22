export interface ListResponse<T> {
    items: T[];
    totalNumberOfItems: number;
    page: number;
    nextPage: string;
    previousPage: string;
}

export interface User {
    id: number;
    firstName: string;
    lastName: string;
    displayName: string;
    username: string;
    email: string;
    profileImageUrl: string;
    coverImageUrl: string;
}

export interface Interaction {
    id: number;
    user: User;
    type: string;
    date: string;
}

export interface Post {
    id: number;
    message: string;
    imageUrl: string;
    postedAt: string;
    postedBy: User;
    likes: Interaction[];
    dislikes: Interaction[];
}

export interface NewPost {
    message: string;
    imageUrl: string;
    userId: number;
}

// export async function fetchLogin(username: string, password: string): Promise<ListResponse<User>> {
export async function fetchLogin(username: string, password: string)  {
    // let usernameendcoded = "kplacido0";
    //  let password = "kplacido0";
    let encodedString = btoa(`${username}:${password}`);
    // console.log (`encoded string = ${encodedString}`);
    const urlString = `https://localhost:5001/login`
    //console.log(`inside fetchlogin  .... urlstring = ${urlString}`)
    const response = await fetch(urlString,
        {
            method: "POST",
            headers: {
                "Authorization" : `Basic ${encodedString}`,
                "Content-Type": "application/json"
            }
        }
    );
    
    if (!response.ok) {
        throw new Error(await response.json());
    }
    else
    {
        //return await response.json();
        console.log(`All ok with the API call ... response is ${response.json}`)
    }
    // return await response.json();

}

export async function fetchUsers(searchTerm: string, page: number, pageSize: number): Promise<ListResponse<User>> {
    let username = "kplacido0";
    let password = "kplacido0";
    let encodedString = btoa(`${username}:${password}`);
    console.log(`encoded string = ${encodedString}`);

    const response = await fetch(`https://localhost:5001/users?search=${searchTerm}&page=${page}&pageSize=${pageSize}`,
        {
            method: "GET",
            headers: {
                "Authorization": `Basic ${encodedString}`,
                "Content-Type": "application/json"
            }
        },
    );
    return await response.json();
}

export async function fetchUser(userId: string | number): Promise<User> {
    const response = await fetch(`https://localhost:5001/users/${userId}`);
    return await response.json();
}

export async function fetchPosts(page: number, pageSize: number): Promise<ListResponse<Post>> {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}`);
    return await response.json();
}

export async function fetchPostsForUser(page: number, pageSize: number, userId: string | number) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&postedBy=${userId}`);
    return await response.json();
}

export async function fetchPostsLikedBy(page: number, pageSize: number, userId: string | number) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&likedBy=${userId}`);
    return await response.json();
}

export async function fetchPostsDislikedBy(page: number, pageSize: number, userId: string | number) {
    const response = await fetch(`https://localhost:5001/feed?page=${page}&pageSize=${pageSize}&dislikedBy=${userId}`);
    return await response.json();
}

export async function createPost(newPost: NewPost) {
    const response = await fetch(`https://localhost:5001/posts/create`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(newPost),
    });

    if (!response.ok) {
        throw new Error(await response.json())
    }
}
