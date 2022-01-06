import { Link } from "react-router-dom"

export default function Genres() {
    document.title = "Game Center - Genres"
    return(
        <>
            <h3>Genres</h3>
            <Link className="btn btn-primary" to="/genres/create">Create Genre</Link>
        </>
    )
}