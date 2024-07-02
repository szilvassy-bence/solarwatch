import City from "./City.jsx";

export default City;

export async function loader(name) {
    try {
        const jsonStorage = window.localStorage.getItem("user");
        let token = "";
        if (jsonStorage != null) {
            const storage = JSON.parse(jsonStorage);
            if (storage && storage.hasOwnProperty("token")){
                token = storage.token;
            }
        }
        const res = await fetch(`/api/SolarWatch/cities/${name}`, {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        if (res.status === 200) {
            const city = await res.json();
            const value = {
                city: city,
                status: 200
            }
            console.log(city);
            return value;
        } else if ( res.status === 404 ) {
            return {status: 404}
        } else {
            return {status: 401}
        }
    } catch (e) {
        console.log(e);
    }
}