import { Footer } from "@/components/footer";
import { Header } from "@/components/header";

export default function GuestLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="grid min-h-dvh grid-rows-[auto_1fr_auto] gap-16">
      <Header />
      {children}
      <Footer />
    </div>
  );
}
