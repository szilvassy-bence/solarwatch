import Cities from "./Cities";
export default Cities;

export async function loader(){
    try {
        const response = await fetch("/api/SolarWatch/GetAllCities");
        const data = await response.json();
        return data;
    } catch (error) {
        console.error("Error fetching city data", error);
    }
}