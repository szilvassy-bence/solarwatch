import Search from "./Search";
export default Search;

export async function loader(searchTerm){
	try {
		const res = await fetch(`/api/SolarWatch/Search/${searchTerm}`);
		const data = await res.json();
		return data;
	} catch (error) {
		console.log(error);
	}
}