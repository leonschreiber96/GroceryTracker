export async function get<T>(url: string): Promise<T> {
   const response = await fetch(url)
   const data = await response.json()

   return data as T
}

export async function post<T_in, T_out>(url: string, content: T_in): Promise<T_out> {
   return new Promise<T_out>((resolve, reject) => {
      console.log("[POST] " + JSON.stringify(content) + ` (${url})`)
      fetch(url, {
         method: "POST",
         headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
         },
         body: JSON.stringify(content),
      })
         .then((response) => response.json())
         .then((val) => val as T_out)
         .then(resolve)
   })
}
