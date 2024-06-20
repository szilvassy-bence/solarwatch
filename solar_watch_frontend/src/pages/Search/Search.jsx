import "./Search.css";
import React from 'react'
import { useLoaderData } from "react-router-dom";
import CityCard from "../../components/CityCard";

const Search = () => {

	const data = useLoaderData();
	console.log(data);

	return (
		<div id="search-wrapper">
			<h1>Search result</h1>
			<div id="search-result-wrapper">
				{data.length > 0 ? (
					data.map(x => (
						<CityCard key={x.id} city={x} />
					))
				) : (
					<h2>No city was found.</h2>
				)}
			</div>
		</div>
	)
}

export default Search