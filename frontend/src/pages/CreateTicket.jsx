import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";

export default function CreateTicket() {

    const navigate = useNavigate();

    const [form, setForm] = useState({
        title: "",
        description: ""
    });

    const [ticket, setTicket] = useState(null);

    const [loading, setLoading] = useState(false);

    const handleChange = (e) => {

        setForm({
            ...form,
            [e.target.name]: e.target.value
        });
    };

    const handleSubmit = async (e) => {

        e.preventDefault();
        setLoading(true);

        try {

            const response =
                await api.post(
                    "/Tickets",
                    form
                );

            setTicket(response.data);

        } catch (error) {

            console.error(error);

        } finally {

            setLoading(false);
        }
    };

    return (
        <div className="create-page">

            <div className="create-container">

                <h1>Create Support Ticket</h1>

                <form onSubmit={handleSubmit}>

                    <input
                        name="title"
                        placeholder="Ticket title"
                        value={form.title}
                        onChange={handleChange}
                        required
                    />

                    <textarea
                        name="description"
                        placeholder="Describe your problem..."
                        value={form.description}
                        onChange={handleChange}
                        rows="7"
                        required
                    />

                    <button
                        type="submit"
                        disabled={loading}
                    >
                        {loading
                            ? "Analyzing..."
                            : "Submit Ticket"}
                    </button>

                </form>

                {ticket && (

                    <div className="prediction-card">

                        <h2>AI Analysis</h2>

                        <div className="prediction-grid">

                            <div>
                                <small>Category</small>
                                <strong>
                                    {ticket.category}
                                </strong>
                            </div>

                            <div>
                                <small>Priority</small>
                                <strong>
                                    {ticket.priority}
                                </strong>
                            </div>

                            <div>
                                <small>Sentiment</small>
                                <strong>
                                    {ticket.sentiment}
                                </strong>
                            </div>

                            <div>
                                <small>Confidence</small>
                                <strong>
                                    {Math.round(
                                        ticket.confidence * 100
                                    )}%
                                </strong>
                            </div>

                        </div>

                        <button
                            onClick={() =>
                                navigate("/dashboard")
                            }
                        >
                            View Dashboard
                        </button>

                    </div>

                )}

            </div>

        </div>
    );
}