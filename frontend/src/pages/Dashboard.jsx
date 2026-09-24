import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import api from "../services/api";

export default function Dashboard() {

    const [tickets, setTickets] = useState([]);

    useEffect(() => {
        loadTickets();
    }, []);

    const loadTickets = async () => {

        try {
            const response =
                await api.get("/Tickets");

            setTickets(response.data);

        } catch (error) {
            console.error(error);
        }
    };

    const open =
        tickets.filter(t => t.status === "Open").length;

    const inProgress =
        tickets.filter(t => t.status === "In Progress").length;

    const resolved =
        tickets.filter(t => t.status === "Resolved").length;

    return (
        <div className="dashboard">

            <header className="topbar">
                <h1>SmartSupport</h1>

                <Link to="/create-ticket">
                    + New Ticket
                </Link>
            </header>

            <main>

                <div className="stats">

                    <div className="stat-card">
                        <span>Total Tickets</span>
                        <strong>{tickets.length}</strong>
                    </div>

                    <div className="stat-card">
                        <span>Open</span>
                        <strong>{open}</strong>
                    </div>

                    <div className="stat-card">
                        <span>In Progress</span>
                        <strong>{inProgress}</strong>
                    </div>

                    <div className="stat-card">
                        <span>Resolved</span>
                        <strong>{resolved}</strong>
                    </div>

                </div>

                <section className="tickets-section">

                    <h2>Recent Tickets</h2>

                    {tickets.length === 0 ? (
                        <p>No tickets yet.</p>
                    ) : (

                        tickets.map(ticket => (

                            <div
                                className="ticket-card"
                                key={ticket.id}
                            >

                                <div>
                                    <h3>
                                        {ticket.title}
                                    </h3>

                                    <p>
                                        {ticket.description}
                                    </p>
                                </div>

                                <div className="ticket-meta">

                                    <span>
                                        {ticket.category}
                                    </span>

                                    <span>
                                        {ticket.priority}
                                    </span>

                                    <span>
                                        {ticket.sentiment}
                                    </span>

                                </div>

                            </div>

                        ))

                    )}

                </section>

            </main>

        </div>
    );
}