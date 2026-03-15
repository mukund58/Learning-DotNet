import './App.css'

function App() {

  const handleCheckout = async () => {
    const res = await fetch("http://localhost:5172/create-checkout", {
      method: "POST"
    });

    if (!res.ok) {
      const err = await res.text();
      console.error("Create checkout failed:", err);
      return;
    }

    const data = await res.json();
    window.location.href = data.url;
  };

  return (
    <button onClick={handleCheckout}>
      Pay Now
    </button>
  );
}

export default App
