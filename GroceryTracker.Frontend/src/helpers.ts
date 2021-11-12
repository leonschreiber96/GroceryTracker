export async function get<T>(url: string): Promise<T> {
   const response = await fetch(url);
   const data = await response.json();

   return data as T;
}

export async function post<T_in, T_out>(url: string, content: T_in): Promise<T_out> {
   console.log(JSON.stringify(content));
   const response = await fetch(url, {
      method: "POST",
      headers: {
         Accept: "application/json",
         "Content-Type": "application/json",
      },
      body: JSON.stringify(content),
   });

   const returnValue = await response.json();

   return returnValue as T_out;
}
